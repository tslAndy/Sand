namespace JobSystem;

public abstract class Container
{
    public MyList<int> inDegree, outDegree;
    public MyList<MyList<JobId>> descendants;

    protected Container() => Preupdate();

    public void Preupdate()
    {
        inDegree    = new MyList<int>(8);
        outDegree   = new MyList<int>(8);
        descendants = new MyList<MyList<JobId>>(8);
        PreupdateInner();
    }

    public abstract void Schedule(int jobIndex);

    protected abstract void PreupdateInner();
}
