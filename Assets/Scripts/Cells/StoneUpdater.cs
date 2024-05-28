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

    public override Color GetColor() => color;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        for (int i = 0; i < 9; i++)
        {
            if (simulation.TrySwap(ref x, ref y, x, y - 1, SwapWithStone)) { }
            else break;
        }
    }
}