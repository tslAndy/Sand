using UnityEngine;

public unsafe class SmokeUpdater : CellUpdater
{
    private Color color = Color.white * 0.5f;
    private const CellType SwapWithSmoke = CellType.Empty |
                                    CellType.Sand |
                                    CellType.Water |
                                    CellType.Gas |
                                    CellType.Fire |
                                    CellType.Acid |
                                    CellType.Oil;

    public SmokeUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => color;
    public override void Update(Cell* cellPtr)
    {
        for (int i = 0; i < 3; i++)
        {
            int x = cellPtr->x;
            int y = cellPtr->y;
            int direction = RandomPosition.PlusMinusOne(0);
            if (simulation.TrySwap(cellPtr, x + direction, y + 1, SwapWithSmoke)) { }
            else if (simulation.TrySwap(cellPtr, x - direction, y + 1, SwapWithSmoke)) { }
            else
            {
                simulation.Remove(x, y);
                break;
            }
        }
    }
}