public struct Extents
{
    private Extent[] extents;

    public Extents()
    {
        extents = new Extent[1024];
        Array.Fill(extents, DefExtent);
    }

    // global coords
    // TODO convert to binary operators
    public void Push(int x, int y)
    {
        Extent extent = this[x, y];
        int tx = x % 32;
        int ty = y % 32;
        extent.minX = (byte)Math.Min(tx, extent.minX);
        extent.maxX = (byte)Math.Max(tx + 1, extent.maxX);
        extent.minY = (byte)Math.Min(ty, extent.minY);
        extent.maxY = (byte)Math.Max(ty + 1, extent.maxY);
        this[x, y] = extent;
    }


    // global coords
    public void Reset(int x, int y) => this[x, y] = new Extent { minX = 32, maxX = 0, minY = 32, maxY = 0 }; 
    private static Extent DefExtent => new Extent { minX = 0, maxX = 32, minY = 0, maxY = 32 };

    // global coords
    // TODO convert to binary operators
    public Extent this[int x, int y]
    {
        get => extents[y / 32 * 32 + x / 32];
        set => extents[y / 32 * 32 + x / 32] = value;
    }
}
