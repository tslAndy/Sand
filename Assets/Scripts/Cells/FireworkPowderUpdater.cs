using UnityEngine;

public unsafe class FireworkPowderUpdater : CellUpdater
{
    private const CellType swapWithPowder = CellType.Empty;
    private Color color = Color.red + Color.magenta;
    public FireworkPowderUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => color;

    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        int height = Random.Range(200, 250);
        for (int i = 0; i < 9; i++)
        {
            if (simulation.HasType(x, y - 1, CellType.Fire))
            {
                simulation.ChangeCellType(cellPtr, CellType.Firework, height);
                return;
            }
            else if (simulation.HasType(x, y + 1, CellType.Fire))
            {
                simulation.ChangeCellType(cellPtr, CellType.Firework, height);
                return;
            }
            else if (simulation.HasType(x + 1, y, CellType.Fire))
            {
                simulation.ChangeCellType(cellPtr, CellType.Firework, height);
                return;
            }
            else if (simulation.HasType(x - 1, y, CellType.Fire))
            {
                simulation.ChangeCellType(cellPtr, CellType.Firework, height);
                return;
            }

            if (simulation.TrySwap(ref x, ref y, x, y - 1, swapWithPowder)) { }
            else if (simulation.TrySwap(ref x, ref y, x - 1, y - 1, swapWithPowder)) { }
            else if (simulation.TrySwap(ref x, ref y, x + 1, y - 1, swapWithPowder)) { }
        }

    }
}