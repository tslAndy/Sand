using UnityEngine;

public unsafe class FiringMaterialUpdater : CellUpdater
{
    public FiringMaterialUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor() => Color.red;
    public override void Update(Cell* cellPtr)
    {
        cellPtr->generation++;
        if (cellPtr->generation == 50)
        {
            cellPtr->cellType = CellType.Fire;
            return;
        }

        int x = cellPtr->x;
        int y = cellPtr->y;

        // try to spawn fire around
        for (int i = 0; i < 5; i++)
        {
            if (Random.Range(0, 100) > 5) continue;

            RandomPosition.GetRandomAround(x, y, out int tx, out int ty);
            if (!simulation.HasType(tx, ty, CellType.Empty)) return;
            simulation.Add(tx, ty, CellType.Fire);
        }
    }
}