using UnityEngine;

public unsafe class GasUpdater : CellUpdater
{
    private const CellType SwapWithGas = CellType.Empty;

    public GasUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => Color.red;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        RandomPosition.GetRandomAround(x, y, out int tx, out int ty);

        simulation.TrySwap(ref x, ref y, tx, ty, SwapWithGas);
    }
}