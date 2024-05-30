using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class FlowerUpdater : CellUpdater
{
    private Color color = Color.green * 0.7f;
    private const int GenerationForFlower = 5;
    public FlowerUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr)
    {
        if (cellPtr->generation > GenerationForFlower) return color;
        return cellPtr->colorscale switch
        {
            0 => Color.blue,
            1 => Color.cyan,
            2 => Color.yellow,
            3 => Color.magenta,
            4 => Color.red,
            5 => Color.white,
            _ => Color.black
        };
    }

    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;
        int generation = cellPtr->generation;

        // same as plant, generation is length
        // stop growing on 5th generation
        // it will be defined in seed

        if (generation > GenerationForFlower)
        {
            bool hasNextElement = simulation.HasType(x - 1, y + 1, CellType.Flower) |
                                simulation.HasType(x, y + 1, CellType.Flower) |
                                simulation.HasType(x + 1, y + 1, CellType.Flower);

            if (hasNextElement) return;
            simulation.TryAdd(RandomPosition.PlusMinusOne(x), y + 1, CellType.Flower, generation - 1, cellPtr->colorscale);

            return;
        }

        // if need to give a flower
        if (generation > 0)
        {
            RandomPosition.GetRandomAround(x, y, out int tx, out int ty);
            simulation.TryAdd(tx, ty, CellType.Flower, generation - 1, cellPtr->colorscale);
        }
    }
}
