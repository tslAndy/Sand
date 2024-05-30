using UnityEngine;

public unsafe class VineUpdater : CellUpdater
{
    private Color color = Color.green * 0.7f;
    public VineUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => color;

    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        if (simulation.HasType(x, y - 1, CellType.Wood))
            simulation.TryAdd(RandomPosition.PlusMinusOne(x), y - 1, CellType.Plant);

        if (cellPtr->generation == 0) return;

        if (Random.Range(0, 10) < 3)
            simulation.TryAdd(x, y - 1, CellType.Vine, cellPtr->generation - 1);
    }
}