using System;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public unsafe abstract class CellUpdater
{
    protected Random rng = new Random(1);
    protected readonly Simulation simulation;

    public CellUpdater(Simulation simulation)
    {
        this.simulation = simulation;
    }

    public abstract Color GetColor();
    public abstract void Update(Cell* cellPtr);
}