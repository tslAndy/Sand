using UnityEngine;

public unsafe class WallUpdater : CellUpdater
{
    public WallUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor() => Color.gray;
    public override void Update(Cell* cellPtr) { }
}