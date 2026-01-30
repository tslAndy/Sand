namespace JobSystem;

public abstract class Allocator
{
    public abstract void Preupdate();
}

public class Allocator<T> : Allocator
{
    private T[] data = new T[8];
    private int fill;

    public override void Preupdate() => fill = 0;

    public int GetOffset(int length)
    {
        if (length > data.Length - fill)
        {
            T[] newData = new T[Math.Max(data.Length * 2, fill + length)];
            Array.Copy(data, newData, fill);
            data = newData;
        }
        int result = fill;
        fill += length;
        return result;
    }

    public void Copy(int sourceId, int destId, int len) => Array.Copy(data, sourceId, data, destId, len);

    public ref T this[int index] => ref data[index];
}