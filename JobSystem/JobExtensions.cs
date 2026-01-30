namespace JobSystem;

public static class JobExtensions
{
    public static JobHandle Schedule<T>(this T job, JobHandle deps = default) where T: struct, IJob
    {
        return JobManager.AddJob(job, deps);
    }

    public static JobHandle Schedule<T>(this T job, int length, int batchesCount, JobHandle deps = default) where T : struct, IJobParallelFor
    {
        return JobManager.AddJobParallelFor(job, length, batchesCount, deps);
    }
}
