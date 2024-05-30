using UnityEngine;

public unsafe class WallUpdater : CellUpdater
{
    public WallUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => Color.gray;
    public override void Update(Cell* cellPtr) { }
}