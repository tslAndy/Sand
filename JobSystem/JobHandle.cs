namespace JobSystem;

public struct JobHandle
{
    public MyList<JobId> jobIds;

    public static JobHandle Combine(JobHandle first, JobHandle second)
    {
        MyList<JobId> jobIds = new MyList<JobId>(first.jobIds, second.jobIds);
        return new JobHandle { jobIds = jobIds };
    }
}