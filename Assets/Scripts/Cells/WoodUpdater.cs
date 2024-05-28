using UnityEngine;

public unsafe class WoodUpdater : CellUpdater
{
    private Color color = (Color.red + Color.yellow) * 0.3f;
    public WoodUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor() => color;
    public override void Update(Cell* cellPtr) { }
}