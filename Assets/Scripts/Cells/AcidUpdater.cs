using UnityEngine;

public unsafe class AcidUpdater : CellUpdater
{
    private const CellType DestroyableByAcid = CellType.Sand |
                                        CellType.Water |
                                        CellType.Oil |
                                        CellType.Wood;

    public const CellType SwapWithAcid = CellType.Empty | CellType.Gas;

    public AcidUpdater(Simulation simulation) : base(simulation) { }

    public override Color GetColor() => rng.NextFloat(0.8f, 1f) * Color.magenta;
    public override void Update(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        // TODO move it to class
        AcidState MoveOrDestroy(int tx, int ty)
        {
            if (tx < 0 || tx > 511 || ty < 0 || ty > 511) return AcidState.Nothing;


            if (simulation.HasType(tx, ty, SwapWithAcid))
            {
                simulation.cellGrid.SwapCells(x, y, tx, ty);
                x = tx;
                y = ty;
                return AcidState.Moved;
            }

            if (simulation.HasType(tx, ty, DestroyableByAcid))
            {
                simulation.Remove(x, y);
                simulation.Remove(tx, ty);
                return AcidState.Destroyed;
            }

            return AcidState.Nothing;
        }

        // TODO fix it
        AcidState acidState;
        for (int i = 0; i < 9; i++)
        {
            acidState = MoveOrDestroy(x, y - 1);
            if (acidState == AcidState.Destroyed) return;
            if (acidState == AcidState.Moved) continue;

            acidState = MoveOrDestroy(x - 1, y - 1);
            if (acidState == AcidState.Destroyed) return;
            if (acidState == AcidState.Moved) continue;

            acidState = MoveOrDestroy(x + 1, y - 1);
            if (acidState == AcidState.Destroyed) return;
            if (acidState == AcidState.Moved) continue;

            acidState = MoveOrDestroy(x - 1, y);
            if (acidState == AcidState.Destroyed) return;
            if (acidState == AcidState.Moved) continue;

            acidState = MoveOrDestroy(x + 1, y);
            if (acidState == AcidState.Destroyed) return;
            if (acidState == AcidState.Moved) continue;

            // if can destroy cell up
            if (simulation.HasType(x, y + 1, DestroyableByAcid))
            {
                simulation.Remove(x, y);
                simulation.Remove(x, y + 1);
                return;
            }

            // if wasn't moved or destroyed
            break;
        }
    }
}

public enum AcidState
{
    Nothing,
    Moved,
    Destroyed
}