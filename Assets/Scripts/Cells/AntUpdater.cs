using UnityEngine;

public unsafe class AntUpdater : CellUpdater
{

    public AntUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => Color.blue;

    // generation 0 is before finding wood
    public override void Update(Cell* cellPtr)
    {
        int vx = cellPtr->vx;
        int vy = cellPtr->vy;

        bool canBuild = cellPtr->generation == 0 ? false : true;
        if (cellPtr->generation == 0)
        {
            for (int i = 0; i < 9; i++)
            {

                if (HasWoodAround(cellPtr->x, cellPtr->y))
                {
                    canBuild = true;
                    cellPtr->generation = 1;

                    RandomPosition.GetRandomAround(0, 0, out vx, out vy);
                    cellPtr->vx = vx;
                    cellPtr->vy = vy;
                    break;
                }

                if (!simulation.TrySwap(cellPtr, cellPtr->x, cellPtr->y - 1, CellType.Empty))
                    break;
            }
        }

        if (cellPtr->generation > Random.Range(80, 200))
        {
            simulation.Remove(cellPtr->x, cellPtr->y);
            return;
        }

        if (!canBuild) return;

        cellPtr->generation++;
        if (Random.Range(0, 10) < 3)
        {
            RandomPosition.GetRandomAround(0, 0, out vx, out vy);
            cellPtr->vx = vx;
            cellPtr->vy = vy;
        }


        int x = cellPtr->x;
        int y = cellPtr->y;

        if (vx != 0 && vy != 0)
        {
            simulation.TryAdd(x - vx, y + vy, CellType.Wood);
            simulation.TryAdd(x + vx, y - vy, CellType.Wood);
        }
        else if (vx != 0)
        {
            simulation.TryAdd(x, y + 1, CellType.Wood);
            simulation.TryAdd(x, y - 1, CellType.Wood);
        }
        else
        {
            simulation.TryAdd(x + 1, y, CellType.Wood);
            simulation.TryAdd(x - 1, y, CellType.Wood);
        }

        if (simulation.HasType(x + vx, y + vy, CellType.Wood))
            simulation.Remove(x + vx, y + vy);

        simulation.TrySwap(cellPtr, x + vx, y + vy, CellType.Empty);
    }

    private bool HasWoodAround(int x, int y)
    {
        if (simulation.HasType(x - 1, y - 1, CellType.Wood))
            return true;
        if (simulation.HasType(x, y - 1, CellType.Wood))
            return true;
        if (simulation.HasType(x + 1, y - 1, CellType.Wood))
            return true;

        if (simulation.HasType(x - 1, y, CellType.Wood))
            return true;
        if (simulation.HasType(x + 1, y - 1, CellType.Wood))
            return true;

        if (simulation.HasType(x - 1, y + 1, CellType.Wood))
            return true;
        if (simulation.HasType(x, y + 1, CellType.Wood))
            return true;
        if (simulation.HasType(x + 1, y + 1, CellType.Wood))
            return true;

        return false;
    }
}
