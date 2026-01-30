namespace JobSystem;

public struct JobHandle
{
    public Shitlist<JobId> jobIds;

    public static JobHandle Combine(JobHandle first, JobHandle second)
    {
        Shitlist<JobId> jobIds = new Shitlist<JobId>(first.jobIds, second.jobIds);
        return new JobHandle { jobIds = jobIds };
    }
}