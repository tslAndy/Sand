namespace JobSystem;

public static class JobManager
{
    private static Dictionary<Type, Container> dict = new();
    private static List<JobId> stack = new List<JobId>(16);
    private static List<JobId> rootJobIds = new List<JobId>(16);

    public static void RunJobs()
    {
        for (int i = 0; i < rootJobIds.Count; i++)
        {
            JobId jobId = rootJobIds[i];
            dict[jobId.type].Schedule(jobId.index);
            stack.Add(jobId);
        }
        rootJobIds.Clear();

        while (stack.Count != 0)
        {
            int stackIndex = 0;
            while (stackIndex < stack.Count)
            {
                JobId jobId = stack[stackIndex];
                Container container = dict[jobId.type];
                if (container.outDegree[jobId.index] != 0)
                {
                    stackIndex++;
                    continue;
                }

                stack[stackIndex] = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);

                MyList<JobId> descendants = container.descendants[jobId.index];
                for (int i = 0; i < descendants.Count; i++)
                {
                    JobId desJobId = descendants[i];
                    Container desContainer = dict[desJobId.type];
                    if (--desContainer.inDegree[desJobId.index] == 0)
                    {
                        desContainer.Schedule(desJobId.index);
                        stack.Add(desJobId);
                    }
                }
            }
        }

        AllocatorVault.Preupdate();
        foreach (Container container in dict.Values)
            container.Preupdate();
    }

    public static JobHandle AddJob<T>(T job, JobHandle deps) where T : struct, IJob
    {
        Type type = typeof(T);
        if (!dict.TryGetValue(type, out Container? container))
        {
            container = new JobContainer<T>();
            dict.Add(type, container);
        }

        JobContainer<T> typedContainer = (JobContainer<T>)container;
        JobHandle handle = typedContainer.AddJob(job, deps);
        JobId jobId = handle.jobIds[0];

        for (int i = 0; i < deps.jobIds.Count; i++)
        {
            JobId dep = deps.jobIds[i];
            dict[dep.type].descendants[dep.index].Add(jobId);
        }

        if (deps.jobIds.Count == 0)
            rootJobIds.Add(jobId);

        return handle;
    }

    public static JobHandle AddJobParallelFor<T>(T job, int length, int batchesCount, JobHandle deps) where T : struct, IJobParallelFor
    {
        Type type = typeof(T);
        if (!dict.TryGetValue(type, out Container? container))
        {
            container = new JobParallelForContainer<T>();
            dict.Add(type, container);
        }

        JobParallelForContainer<T> typedContainer = (JobParallelForContainer<T>)container;
        JobHandle handle = typedContainer.AddJob(job, length, batchesCount, deps);

        JobId jobId = handle.jobIds[0];
        for (int i = 0; i < deps.jobIds.Count; i++)
        {
            JobId dep = deps.jobIds[i];
            dict[dep.type].descendants[dep.index].Add(jobId);
        }

        if (deps.jobIds.Count == 0)
            rootJobIds.Add(jobId);

        return handle;
    }
}
