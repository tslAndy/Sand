using UnityEngine;

public unsafe class OilUpdater : CellUpdater
{
    private const CellType SwapWithOil = CellType.Empty | CellType.Gas;

    public OilUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => rng.NextFloat(0.8f, 1f) * Color.green;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        int direction = 1;

        for (int i = 0; i < 9; i++)
        {
            if (simulation.TrySwap(ref x, ref y, x, y - 1, SwapWithOil)) { }
            else if (simulation.TrySwap(ref x, ref y, x + direction, y - 1, SwapWithOil)) { }
            else if (simulation.TrySwap(ref x, ref y, x - direction, y - 1, SwapWithOil))
                direction = -direction;
            else if (simulation.TrySwap(ref x, ref y, x + direction, y, SwapWithOil)) { }
            else if (simulation.TrySwap(ref x, ref y, x - direction, y, SwapWithOil))
                direction = -direction;
            else break;
        }
    }
}