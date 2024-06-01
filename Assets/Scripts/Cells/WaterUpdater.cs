using UnityEngine;


public unsafe class WaterUpdater : CellUpdater
{
    private const CellType SwapWithWater = CellType.Empty |
                                        CellType.Oil |
                                        CellType.Gas;

    public WaterUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => rng.NextFloat(0.8f, 1f) * Color.blue;

    public override void Update(Cell* cellPtr)
    {
        int direction = 1;

        for (int i = 0; i < 9; i++)
        {
            int x = cellPtr->x;
            int y = cellPtr->y;


            if (simulation.TrySwap(cellPtr, x, y - 1, SwapWithWater)) { }
            else if (simulation.TrySwap(cellPtr, x + direction, y - 1, SwapWithWater)) { }
            else if (simulation.TrySwap(cellPtr, x - direction, y - 1, SwapWithWater))
                direction = -direction;
            else if (simulation.TrySwap(cellPtr, x + direction, y, SwapWithWater)) { }
            else if (simulation.TrySwap(cellPtr, x - direction, y, SwapWithWater))
                direction = -direction;
            else break;
        }
    }
}