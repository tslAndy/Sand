using UnityEngine;

public unsafe class FireUpdater : CellUpdater
{
    private const CellType CanDetonate = CellType.Gas;
    private const CellType SwapWithFire = CellType.Empty;
    private const CellType CanFire = CellType.Oil | CellType.Wood;

    private Color _color = (Color.red + Color.yellow) * 0.5f;

    public FireUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor() => _color;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        if (cellPtr->lifetime == 100)
        {
            if (Random.Range(0, 100) < 20)
                cellPtr->cellType = CellType.Smoke;
            else
                simulation.Remove(x, y);
            return;
        }

        cellPtr->lifetime++;

        // try to fire smth around
        for (int i = 0; i < 5; i++)
        {
            int tx = x + Random.Range(-1, 2);
            int ty = y + Random.Range(-1, 2);

            if (simulation.HasType(tx, ty, CanFire))
                simulation.cellGrid[ty, tx]->cellType = CellType.FiringMaterial;
            else if (simulation.HasType(tx, ty, CanDetonate))
            {
                Cell* explodedPtr = simulation.cellGrid[ty, tx];
                explodedPtr->cellType = CellType.Explosion;
            }
        }

        simulation.TrySwap(ref x, ref y, x + Random.Range(-1, 2), y + 1, SwapWithFire);
    }
}