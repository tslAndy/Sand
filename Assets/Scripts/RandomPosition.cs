using UnityEngine;

public static class RandomPosition
{
    public static void GetRandomAround(int x, int y, out int tx, out int ty)
    {
        // 50 by 50
        if (Random.Range(0, 2) == 0)
        {
            tx = PlusMinusOne(x);
            ty = y + Random.Range(-1, 2);
        }
        else
        {
            tx = x + Random.Range(-1, 2);
            ty = PlusMinusOne(y);
        }
    }

    public static int PlusMinusOne(int n) => n + Random.Range(0, 2) * 2 - 1;
}