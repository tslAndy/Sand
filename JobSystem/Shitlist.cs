namespace JobSystem;

public struct Shitlist<T>
{
    private Allocator<T> allocator;
    private int offset, count, capacity;

    public Shitlist(int capacity)
    {
        this.allocator = AllocatorVault.GetAllocator<T>();
        this.capacity  = capacity;
        this.offset    = allocator.GetOffset(capacity);
    }

    public Shitlist(Shitlist<T> first, Shitlist<T> second)
    {
        allocator = AllocatorVault.GetAllocator<T>();
        capacity  = first.capacity + second.capacity;
        count     = capacity;
        offset    = allocator.GetOffset(capacity);
        allocator.Copy(first.offset, offset, first.count);
        allocator.Copy(second.offset, offset + first.count, second.count);
    }

    public void Add(T elem)
    {
        if (count == capacity)
        {
            int newOffset = allocator.GetOffset(capacity * 2);
            allocator.Copy(offset, newOffset, capacity);
            offset   =  newOffset;
            capacity *= 2;
        }

        this[count] = elem;
        count++;
    }

    public int Count => count;

    public ref T this[int index] => ref allocator[offset + index];
}
