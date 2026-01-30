namespace JobSystem;

public static class AllocatorVault
{
    private static Dictionary<Type, Allocator> dict = new();

    public static Allocator<T> GetAllocator<T>()
    {
        if (!dict.TryGetValue(typeof(T), out Allocator? alloc))
        {
            alloc = new Allocator<T>();
            dict.Add(typeof(T), alloc);
        }
        return (Allocator<T>)alloc;
    }

    public static void Preupdate()
    {
        foreach (Allocator alloc in dict.Values)
            alloc.Preupdate();
    }
}