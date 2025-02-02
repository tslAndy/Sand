using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public unsafe class SimulationUpdater : MonoBehaviour
{
    [SerializeField] private Simulation simulation;

    public Dictionary<CellType, CellUpdater> cellUpdaters;

    private void Awake()
    {
        Application.targetFrameRate = 70;
    }

    private void Start()
    {
        cellUpdaters = new()
        {
            {CellType.Sand, new SandUpdater(simulation)},
            {CellType.Water, new WaterUpdater(simulation)},
            {CellType.Gas, new GasUpdater(simulation)},
            {CellType.Fire, new FireUpdater(simulation)},
            {CellType.Wall, new WallUpdater(simulation)},
            {CellType.Acid, new AcidUpdater(simulation)},
            {CellType.Oil, new OilUpdater(simulation)},
            {CellType.Wood, new WoodUpdater(simulation)},
            {CellType.FiringMaterial, new FiringMaterialUpdater(simulation)},
            {CellType.Explosion, new ExplosionUpdater(simulation)},
            {CellType.Smoke, new SmokeUpdater(simulation)},
            {CellType.Stone, new StoneUpdater(simulation)},
            {CellType.Ice, new IceUpdater(simulation)},
            {CellType.Plant, new PlantUpdater(simulation)},
            {CellType.Vine, new VineUpdater(simulation)},
            {CellType.Flower, new FlowerUpdater(simulation)},
            {CellType.Seed, new SeedUpdater(simulation)},
            {CellType.FireworkPowder, new FireworkPowderUpdater(simulation)},
            {CellType.Firework, new FireworkUpdater(simulation)},
            {CellType.Sparkle, new SparkleUpdater(simulation)},
            {CellType.Mite, new MiteUpdater(simulation)},
            {CellType.Ant, new AntUpdater(simulation)}


        };
    }

    private void Update()
    {
        Debug.Log(simulation.Length);
        for (int i = 0; i < simulation.Length; i++)
        {
            Cell* cellPtr = simulation[i];
            cellUpdaters[cellPtr->cellType].Update(cellPtr);
        }
    }
}
