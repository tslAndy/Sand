using UnityEngine;

public unsafe class FireworkUpdater : CellUpdater
{
    private const CellType swapWithFirework = CellType.Empty | CellType.Fire | CellType.Smoke;
    private Color color = Color.blue;
    public FireworkUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => color;

    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;
        Debug.Log($"Generation: {cellPtr->generation}");

        for (int i = 0; i < 3; i++)
        {
            if (cellPtr->generation != 0 && simulation.TrySwap(ref x, ref y, x, y + 1, swapWithFirework))
            {
                cellPtr->generation--;
                continue;
            }
            int radius = Random.Range(40, 100);
            int colorscale = Random.Range(0, 6);

            simulation.TryAdd(x, y + 2, CellType.Sparkle, radius, colorscale, 0, 7);
            simulation.TryAdd(x + 2, y + 2, CellType.Sparkle, radius, colorscale, 5, 5);
            simulation.TryAdd(x + 2, y, CellType.Sparkle, radius, colorscale, 7, 0);
            simulation.TryAdd(x + 2, y - 2, CellType.Sparkle, radius, colorscale, 5, -5);
            simulation.TryAdd(x, y - 2, CellType.Sparkle, radius, colorscale, 0, -7);
            simulation.TryAdd(x - 2, y - 2, CellType.Sparkle, radius, colorscale, -5, -5);
            simulation.TryAdd(x - 2, y, CellType.Sparkle, radius, colorscale, -7, 0);
            simulation.TryAdd(x - 2, y + 2, CellType.Sparkle, radius, colorscale, -5, 5);
            simulation.Remove(x, y);
            return;
        }
    }
}