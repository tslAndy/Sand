using UnityEngine;

public unsafe class SandUpdater : CellUpdater
{
    private const CellType SwapWithSand = CellType.Empty |
                                        CellType.Water |
                                        CellType.Gas |
                                        CellType.Oil;

    public SandUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => Color.yellow;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        for (int i = 0; i < 9; i++)
        {
            if (simulation.TrySwap(ref x, ref y, x, y - 1, SwapWithSand)) { }
            else if (simulation.TrySwap(ref x, ref y, x - 1, y - 1, SwapWithSand)) { }
            else if (simulation.TrySwap(ref x, ref y, x + 1, y - 1, SwapWithSand)) { }
            else break;
        }
    }
}