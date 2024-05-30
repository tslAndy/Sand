using UnityEngine;

public unsafe class FireUpdater : CellUpdater
{
    private const CellType CanDetonate = CellType.Gas;
    private const CellType SwapWithFire = CellType.Empty;
    private const CellType CanFire = CellType.Oil | CellType.Wood;

    private Color _color = (Color.red + Color.yellow) * 0.5f;

    public FireUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => _color;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        if (cellPtr->generation == 100)
        {
            if (Random.Range(0, 100) < 20)
                cellPtr->cellType = CellType.Smoke;
            else
                simulation.Remove(x, y);
            return;
        }

        cellPtr->generation++;

        // try to fire smth around
        for (int i = 0; i < 5; i++)
        {
            RandomPosition.GetRandomAround(x, y, out int tx, out int ty);

            if (simulation.HasType(tx, ty, CanFire, out Cell* firedPtr))
                simulation.ChangeCellType(firedPtr, CellType.FiringMaterial);
            else if (simulation.HasType(tx, ty, CanDetonate, out Cell* detonatedPtr))
                simulation.ChangeCellType(detonatedPtr, CellType.Explosion);
        }

        simulation.TrySwap(ref x, ref y, RandomPosition.PlusMinusOne(x), y + 1, SwapWithFire);
    }
}