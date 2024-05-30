using UnityEngine;

public unsafe class ExplosionUpdater : CellUpdater
{
    private const CellType ThrowableByDetonation = CellType.Gas;
    public ExplosionUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor() => Color.red;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        int generation = cellPtr->generation;

        //ExplodeInDirection(x, y, vx, vy);

        int dx = (int)Mathf.Sign(Random.Range(-1, 2));
        int dy = (int)Mathf.Sign(Random.Range(-1, 2));
        ExplodeInDirection(x, y, dx, dy);

        dx = (int)Mathf.Sign(Random.Range(-1, 2));
        dy = (int)Mathf.Sign(Random.Range(-1, 2));
        ExplodeInDirection(x, y, dx, dy);

        if (generation < 20) simulation.Remove(x, y);
        else simulation.ChangeCellType(cellPtr, CellType.Fire);
    }

    private void ExplodeInDirection(int x, int y, int dx, int dy)
    {
        var tx = x + dx;
        var ty = y + dy;
        int generation = simulation[y, x]->generation;
        bool isLastGeneration = generation == 20;

        if (!isLastGeneration &&
            simulation.HasType(tx, ty, CellType.Empty))
        {
            simulation.Add(tx, ty, CellType.Explosion, generation + 1);
        }
        if (!isLastGeneration &&
            simulation.HasType(tx, ty, ThrowableByDetonation) &&
            simulation.HasType(tx + dx, ty + dy, CellType.Empty))
        {
            simulation.SwapCells(tx, ty, tx + dx, ty + dy);
        }
    }
}