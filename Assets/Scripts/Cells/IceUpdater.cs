using UnityEngine;

public unsafe class IceUpdater : CellUpdater
{
    private Color color = (Color.white + Color.blue) * 0.6f;
    public IceUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => color;
    public override void Update(Cell* cellPtr)
    {
        if (cellPtr->generation < 40)
        {
            cellPtr->generation++;
            return;
        }

        int x = cellPtr->x;
        int y = cellPtr->y;

        int steps = Random.Range(0, 5);
        for (int i = 0; i < steps; i++)
        {
            RandomPosition.GetRandomAround(x, y, out int tx, out int ty);
            if (simulation.HasType(tx, ty, CellType.Water, out Cell* waterPtr))
            {
                simulation.ChangeCellType(waterPtr, CellType.Ice);
                x = tx;
                y = ty;
            }
        }
    }
}