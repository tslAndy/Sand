using UnityEngine;

public unsafe class PlantUpdater : CellUpdater
{
    private Color color = Color.green * 0.7f;
    public PlantUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor() => color;

    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        for (int i = 0; i < 4; i++)
        {
            RandomPosition.GetRandomAround(x, y, out int tx, out int ty);
            if (simulation.HasType(tx, ty, CellType.Wood))
            {
                int dx = tx - x;
                int dy = ty - y;

                if (dx != 0 && dy != 0)
                {
                    simulation.TryAdd(tx, y, CellType.Plant);
                    simulation.TryAdd(x, ty, CellType.Plant);
                }
                else if (dx != 0)
                {
                    simulation.TryAdd(x, RandomPosition.PlusMinusOne(y), CellType.Plant);
                }
                else
                    simulation.TryAdd(RandomPosition.PlusMinusOne(x), y, CellType.Plant);
            }
            else if (cellPtr->generation < 5 && simulation.HasType(tx, ty, CellType.Empty))
            {
                simulation.TryAdd(tx, ty, CellType.Plant, cellPtr->generation + 1);
            }
        }
    }
}