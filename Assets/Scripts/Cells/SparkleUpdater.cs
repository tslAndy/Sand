using System;
using System.IO.Compression;
using Unity.Mathematics;
using UnityEngine;

public unsafe class SparkleUpdater : CellUpdater
{
    private const CellType swapWithSparkle = CellType.Empty | CellType.Fire | CellType.Smoke | CellType.Sparkle;

    public SparkleUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => cellPtr->colorscale switch
    {
        0 => Color.blue,
        1 => Color.cyan,
        2 => Color.yellow,
        3 => Color.magenta,
        4 => Color.red,
        5 => Color.white,
        _ => Color.black
    };


    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        int vx = cellPtr->vx;
        int vy = cellPtr->vy;

        if (Mathf.Abs(vx) > Mathf.Abs(vy))
            Move(cellPtr, Math.Sign(vx), vy / (float)vx, (int)MathF.Abs(vx));
        else
            Move(cellPtr, vx / (float)vy, Math.Sign(vy), (int)Math.Abs(vy));
    }

    private void Move(Cell* cellPtr, float dx, float dy, int count)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        int x0 = x;
        int y0 = y;

        for (int i = 1; i < count + 1; i++)
        {
            int tx = x0 + Mathf.RoundToInt(dx * i);
            int ty = y0 + Mathf.RoundToInt(dy * i);
            if (cellPtr->generation == 0 || !simulation.TrySwap(ref x, ref y, tx, ty, swapWithSparkle))
            {
                simulation.Remove(x, y);
                break;
            }
            cellPtr->generation--;
        }
    }
}