using System;
using UnityEngine;
using Random = UnityEngine.Random;

public unsafe class MiteUpdater : CellUpdater
{
    private CellType destroyableByMite = CellType.Wood | CellType.Plant | CellType.Vine | CellType.Flower;
    private CellType swapWithMite = CellType.Empty;
    private Color color = Color.blue;
    public MiteUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => color;

    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        bool eaten = false;
        for (int i = 0; i < 3; i++)
        {
            if (Random.Range(0, 10) > 3) break;
            RandomPosition.GetRandomAround(x, y, out int tx, out int ty);
            if (!simulation.HasType(tx, ty, destroyableByMite)) continue;

            eaten = true;
            simulation.Remove(tx, ty);
            simulation.TrySwap(ref x, ref y, tx, ty, swapWithMite);
        }

        if (eaten) return;

        if (cellPtr->generation == 0)
        {
            for (int i = 0; i < 9; i++)
            {
                bool falledDown = simulation.TrySwap(ref x, ref y, x, y - 1, swapWithMite);
                if (falledDown) continue;

                cellPtr->vx = RandomPosition.PlusMinusOne(0);
                cellPtr->vy = 1;
                cellPtr->generation = 1;
            }
            return;
        }

        if (cellPtr->vy > 0 && cellPtr->generation > Random.Range(5, 10))
            cellPtr->vy = -1;

        if (cellPtr->generation > Random.Range(15, 20))
            cellPtr->vx = 0;

        if (simulation.TrySwap(ref x, ref y, x + cellPtr->vx, y + cellPtr->vy, swapWithMite))
            cellPtr->generation++;
        else
            cellPtr->generation = 0;
    }

}