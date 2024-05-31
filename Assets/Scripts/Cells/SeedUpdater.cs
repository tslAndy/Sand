using UnityEngine;

public unsafe class SeedUpdater : CellUpdater
{
    private Color color = Color.blue + Color.magenta * 0.6f;
    private const CellType SwapWithSeed = CellType.Empty |
                                        CellType.Water |
                                        CellType.Gas |
                                        CellType.Oil;

    // todo add plant, flower
    private const CellType GrowOn = CellType.Sand | CellType.Flower | CellType.Plant;

    public SeedUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor(Cell* cellPtr) => color;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        if (simulation.HasType(x, y - 1, GrowOn, out Cell* growOnPtr))
        {
            simulation.Remove(x, y);
            simulation.ChangeCellType(growOnPtr, CellType.Flower, Random.Range(20, 40), Random.Range(0, 6));
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            if (simulation.TrySwap(ref x, ref y, x, y - 1, SwapWithSeed)) { }
            else if (simulation.TrySwap(ref x, ref y, x - 1, y - 1, SwapWithSeed)) { }
            else if (simulation.TrySwap(ref x, ref y, x + 1, y - 1, SwapWithSeed)) { }
            else break;
        }
    }
}