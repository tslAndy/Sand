namespace JobSystem;

public class JobContainer<T> : Container
    where T : struct, IJob
{
    private MyList<T> jobs;

    public JobHandle AddJob(T job, JobHandle deps)
    {
        MyList<JobId> jobIds = new MyList<JobId>(1);
        jobIds.Add(new JobId { type = typeof(T), index = jobs.Count });

        inDegree.Add(deps.jobIds.Count);
        outDegree.Add(1);
        descendants.Add(new MyList<JobId>(1));
        jobs.Add(job);

        return new JobHandle { jobIds = jobIds };
    }

    public override void Schedule(int jobIndex)
    {
        JobData data = new JobData { container = this, jobIndex = jobIndex };

        ThreadPool.QueueUserWorkItem<JobData>(
            x =>
            {
                ref T job = ref x.container.jobs[x.jobIndex];
                job.Execute();
                Interlocked.Add(ref x.container.outDegree[x.jobIndex], -1);
            },
            data,
            false
        );
    }

    protected override void PreupdateInner() => jobs = new MyList<T>(8);

    private struct JobData
    {
        public JobContainer<T> container;
        public int jobIndex;
    }
}
