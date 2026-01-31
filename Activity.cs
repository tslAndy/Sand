
public struct Activity
{
    private int[] activity;

    public Activity()
    {
        activity = new int[32];
        Array.Fill(activity, ~0);
    }

    // global coords
    public bool this[int x, int y]
    {
        get => ((activity[x / 32] >> (y / 32)) & 1) == 1;
        set
        {
            x /= 32;
            y /= 32;

            int mask = 1 << y;

            if (!value)
            {
                Interlocked.And(ref activity[x], ~mask);
                // activity[x] &= ~mask;
                return;
            }

            mask |= (mask << 1) | (mask >> 1);
            activity[x] |= mask;
            if (x - 1 >= 0)
                Interlocked.Or(ref activity[x - 1], mask);
            // activity[x - 1] |= mask;
            if (x + 1 < 32)
                Interlocked.Or(ref activity[x + 1], mask);
            // activity[x + 1] |= mask;

            // TODO check if Interlocked is required here
        }
    }
}
