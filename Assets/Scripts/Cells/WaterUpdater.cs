using UnityEngine;

public unsafe class WaterUpdater : CellUpdater
{
    private const CellType SwapWithWater = CellType.Empty |
                                        CellType.Oil |
                                        CellType.Gas;

    public WaterUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor() => Color.blue;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        int direction = 1;

        for (int i = 0; i < 9; i++)
        {
            if (simulation.TrySwap(ref x, ref y, x, y - 1, SwapWithWater)) { }
            else if (simulation.TrySwap(ref x, ref y, x + direction, y - 1, SwapWithWater)) { }
            else if (simulation.TrySwap(ref x, ref y, x - direction, y - 1, SwapWithWater))
                direction = -direction;
            else if (simulation.TrySwap(ref x, ref y, x + direction, y, SwapWithWater)) { }
            else if (simulation.TrySwap(ref x, ref y, x - direction, y, SwapWithWater))
                direction = -direction;
            else break;
        }
    }
}