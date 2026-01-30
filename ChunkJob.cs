using JobSystem;

public struct ChunkJob : IJobParallelFor
{
    public Activity activity;
    public Extents extents;
    public Logic logic;
    public int offsetY, baseOffsetX;
    public bool forward;

    public void Execute(int index)
    {
        int y = offsetY;
        int x = baseOffsetX + index * 128;
        UpdateSubchunk(x, y);
        UpdateSubchunk(x + 32, y);
        UpdateSubchunk(x, y + 32);
        UpdateSubchunk(x + 32, y + 32);
    }

    private void UpdateSubchunk(int x, int y)
    {
        if (!activity[x, y])
            return;

        Extent extent = extents[x, y];
        extents.Reset(x, y);

        int startX = x + extent.minX;
        int endX = x + extent.maxX;
        int startY = y + extent.minY;
        int endY = y + extent.maxY;

        Traverse travCur = new Traverse { sx = startX, ex = endX, delta = 1 };
        Traverse travNext = new Traverse { sx = endX - 1, ex = startX - 1, delta = -1 };

        if (forward)
            (travCur, travNext) = (travNext, travCur);

        bool activated = false;
        for (int ty = startY; ty < endY; ty++)
        {
            int tx = travCur.sx;
            while (tx != travCur.ex)
            {
                activated |= logic.Update(tx, ty);
                tx += travCur.delta;
            }
            (travCur, travNext) = (travNext, travCur);
        }

        activity[x, y] = activated;
    }

    private struct Traverse { public int sx, ex, delta; }
}
