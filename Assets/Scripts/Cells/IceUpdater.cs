using UnityEngine;

public unsafe class IceUpdater : CellUpdater
{
    private Color color = Color.white + Color.blue * 0.6f;
    public IceUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor() => color;
    public override void Update(Cell* cellPtr)
    {
        if (cellPtr->lifetime < 40)
        {
            cellPtr->lifetime++;
            return;
        }

        int x = cellPtr->x;
        int y = cellPtr->y;

        //if (Random.Range(0, 100) > 20) return;

        int steps = Random.Range(0, 10);
        for (int i = 0; i < steps; i++)
        {
            RandomPosition.GetRandomAround(x, y, out int tx, out int ty);
            if (simulation.HasType(tx, ty, CellType.Water))
            {
                simulation.cellGrid[ty, tx]->cellType = CellType.Ice;
                x = tx;
                y = ty;
            }
        }
    }
}