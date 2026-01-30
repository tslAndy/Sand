namespace JobSystem;

public class JobParallelForContainer<T> : Container where T: struct, IJobParallelFor
{
    private MyList<T>     jobs;
    private MyList<Range> ranges;

    public JobHandle AddJob(T job, int length, int batchesCount, JobHandle deps)
    {
        MyList<JobId> jobIds = new MyList<JobId>(1);
        jobIds.Add(new JobId { type = typeof(T), index = jobs.Count });

        inDegree.Add(deps.jobIds.Count);
        outDegree.Add(batchesCount);
        descendants.Add(new MyList<JobId>(1));
        ranges.Add(new Range { length = length, batchesCount = batchesCount });
        jobs.Add(job);

        return new JobHandle { jobIds = jobIds };
    }

    public override void Schedule(int jobIndex)
    {
        int batchesCount = ranges[jobIndex].batchesCount;
        for (int i = 0; i < batchesCount; i++)
        {
            JobData data = new JobData
            {
                container  = this,
                jobIndex   = jobIndex,
                batchIndex = i
            };

            ThreadPool.QueueUserWorkItem<JobData>(
                x =>
                {
                    ref T job      = ref x.container.jobs[x.jobIndex];
                    Range range    = x.container.ranges[x.jobIndex];
                    int   batchLen = range.length / range.batchesCount;
                    int   offset   = x.batchIndex * batchLen;
                    for (int j = 0; j < batchLen; j++)
                        job.Execute(offset + j);
                    Interlocked.Add(ref x.container.outDegree[x.jobIndex], -1);
                },
                data,
                false
            );
        }
    }

    protected override void PreupdateInner()
    {
        jobs   = new(8);
        ranges = new(8);
    }

    private struct JobData
    {
        public JobParallelForContainer<T> container;
        public int jobIndex, batchIndex;
    }

    private struct Range
    {
        public int length, batchesCount;
    }
}
