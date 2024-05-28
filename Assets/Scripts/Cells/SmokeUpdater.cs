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

    public override Color GetColor() => color;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        for (int i = 0; i < 3; i++)
        {
            int direction = Random.Range(-1, 2);
            if (simulation.TrySwap(ref x, ref y, x + direction, y + 1, SwapWithSmoke)) { }
            else if (simulation.TrySwap(ref x, ref y, x - direction, y + 1, SwapWithSmoke)) { }
            else
            {
                simulation.Remove(x, y);
                break;
            }
        }
    }
}