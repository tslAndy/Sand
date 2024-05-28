using UnityEngine;

public unsafe class GasUpdater : CellUpdater
{
    private const CellType SwapWithGas = CellType.Empty;

    public GasUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor() => Color.red;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        int dx = Random.Range(-1, 2);
        int dy = Random.Range(-1, 2);

        simulation.TrySwap(ref x, ref y, x + dx, y + dy, SwapWithGas);
    }
}