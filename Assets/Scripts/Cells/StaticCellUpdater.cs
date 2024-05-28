using UnityEngine;

public unsafe class StaticCellUpdater : CellUpdater
{

    public StaticCellUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor() => Color.black;
    public override void Update(Cell* cellPtr) { }
}