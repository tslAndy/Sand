namespace JobSystem;

public abstract class Container
{
    public Shitlist<int> inDegree, outDegree;
    public Shitlist<Shitlist<JobId>> descendants;

    protected Container() => Preupdate();

    public void Preupdate()
    {
        inDegree    = new Shitlist<int>(8);
        outDegree   = new Shitlist<int>(8);
        descendants = new Shitlist<Shitlist<JobId>>(8);
        PreupdateInner();
    }

    public abstract void Schedule(int jobIndex);

    protected abstract void PreupdateInner();
}
