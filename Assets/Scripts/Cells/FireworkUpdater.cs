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

        for (int i = 0; i < 4; i++)
        {
            if (cellPtr->generation != 0 && simulation.TrySwap(ref x, ref y, x, y + 1, swapWithFirework))
            {
                cellPtr->generation--;
                continue;
            }
            int radius = Random.Range(40, 100);
            int colorscale = Random.Range(0, 6);

            AddSymmetric(x, y, dx: 0, dy: 2, vx: 0, vy: 7, radius, colorscale);
            AddSymmetric(x, y, dx: 1, dy: 2, vx: 3, vy: 7, radius, colorscale);
            AddSymmetric(x, y, dx: 2, dy: 2, vx: 5, vy: 5, radius, colorscale);
            AddSymmetric(x, y, dx: 2, dy: 1, vx: 7, vy: 3, radius, colorscale);
            AddSymmetric(x, y, dx: 2, dy: 0, vx: 7, vy: 0, radius, colorscale);

            simulation.Remove(x, y);
            return;
        }

    }

    private void AddSymmetric(
        int x, int y,
        int dx, int dy,
        int vx, int vy,
        int generation, int colorscale)
    {
        if (dx != 0 && dy != 0)
        {
            simulation.TryAdd(x + dx, y + dy, CellType.Sparkle, generation, colorscale, vx, vy);
            simulation.TryAdd(x + dx, y - dy, CellType.Sparkle, generation, colorscale, vx, -vy);

            simulation.TryAdd(x - dx, y + dy, CellType.Sparkle, generation, colorscale, -vx, vy);
            simulation.TryAdd(x - dx, y - dy, CellType.Sparkle, generation, colorscale, -vx, -vy);
        }
        else if (dx == 0)
        {
            simulation.TryAdd(x, y + dy, CellType.Sparkle, generation, colorscale, vx, vy);
            simulation.TryAdd(x, y - dy, CellType.Sparkle, generation, colorscale, vx, -vy);
        }
        else
        {
            simulation.TryAdd(x + dx, y, CellType.Sparkle, generation, colorscale, vx, vy);
            simulation.TryAdd(x - dx, y, CellType.Sparkle, generation, colorscale, -vx, vy);
        }
    }
}