using UnityEngine;

public unsafe class StoneUpdater : CellUpdater
{
    private Color color = Color.white * 0.7f;
    private const CellType SwapWithStone = CellType.Empty |
                                    CellType.Water |
                                    CellType.Gas |
                                    CellType.Fire |
                                    CellType.Acid |
                                    CellType.Oil;

    public StoneUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => color;
    public override void Update(Cell* cellPtr)
    {
        for (int i = 0; i < 9; i++)
        {
            int x = cellPtr->x;
            int y = cellPtr->y;
            if (simulation.TrySwap(cellPtr, x, y - 1, SwapWithStone)) { }
            else break;
        }
    }
}