using UnityEngine;

public unsafe abstract class CellUpdater
{
    protected readonly Simulation simulation;

    public CellUpdater(Simulation simulation)
    {
        this.simulation = simulation;
    }

    public abstract Color GetColor();
    public abstract void Update(Cell* cellPtr);
}