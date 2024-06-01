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
        for (int i = 0; i < 9; i++)
        {
            if (simulation.TrySwap(cellPtr, cellPtr->x, cellPtr->y - 1, SwapWithSand)) { }
            else if (simulation.TrySwap(cellPtr, cellPtr->x - 1, cellPtr->y - 1, SwapWithSand)) { }
            else if (simulation.TrySwap(cellPtr, cellPtr->x + 1, cellPtr->y - 1, SwapWithSand)) { }
        }
    }
}