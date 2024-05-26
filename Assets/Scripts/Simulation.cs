using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public unsafe class Simulation : MonoBehaviour
{
    public CellArray cellArray = new();
    public CellGrid cellGrid = new();


    #region swap constants
    private const CellType swapWithSand = CellType.Empty | CellType.Water | CellType.Gas | CellType.Oil;
    private const CellType swapWithWater = CellType.Empty | CellType.Oil | CellType.Gas;
    private const CellType swapWithGas = CellType.Empty;
    private const CellType swapWithOil = CellType.Empty | CellType.Gas;

    private const CellType destroyableByAcid = CellType.Sand | CellType.Water | CellType.Oil | CellType.Wood;
    private const CellType swapWithAcid = CellType.Empty | CellType.Gas;

    private const CellType swapWithFire = CellType.Empty;
    private const CellType destroyableByFire = CellType.Oil | CellType.Wood;
    #endregion


    public void Update()
    {
        for (int i = 0; i < cellArray.Length; i++)
        {
            Cell* cell = cellArray[i];

            switch (cell->cellType)
            {
                case CellType.Sand:
                    UpdateSand(cell);
                    break;

                case CellType.Gas:
                    UpdateGas(cell);
                    break;

                case CellType.Water:
                    UpdateWater(cell);
                    break;

                case CellType.Oil:
                    UpdateOil(cell);
                    break;

                case CellType.Acid:
                    UpdateAcid(cell);
                    break;

                case CellType.Fire:
                    UpdateFire(cell);
                    break;


                default:
                    break;
            }
        }
    }

    #region Update Methods
    private void UpdateSand(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        for (int i = 0; i < 9; i++)
        {
            if (TrySwap(ref x, ref y, x, y - 1, swapWithSand)) { }
            else if (TrySwap(ref x, ref y, x - 1, y - 1, swapWithSand)) { }
            else if (TrySwap(ref x, ref y, x + 1, y - 1, swapWithSand)) { }
            else break;
        }
    }

    private void UpdateWater(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        int direction = 1;

        for (int i = 0; i < 9; i++)
        {
            if (TrySwap(ref x, ref y, x, y - 1, swapWithWater)) { }
            else if (TrySwap(ref x, ref y, x + direction, y - 1, swapWithWater)) { }
            else if (TrySwap(ref x, ref y, x - direction, y - 1, swapWithWater))
                direction = -direction;
            else if (TrySwap(ref x, ref y, x + direction, y, swapWithWater)) { }
            else if (TrySwap(ref x, ref y, x - direction, y, swapWithWater))
                direction = -direction;
            else break;
        }
    }

    private void UpdateOil(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        int direction = 1;

        for (int i = 0; i < 9; i++)
        {
            if (TrySwap(ref x, ref y, x, y - 1, swapWithOil)) { }
            else if (TrySwap(ref x, ref y, x + direction, y - 1, swapWithOil)) { }
            else if (TrySwap(ref x, ref y, x - direction, y - 1, swapWithOil))
                direction = -direction;
            else if (TrySwap(ref x, ref y, x + direction, y, swapWithOil)) { }
            else if (TrySwap(ref x, ref y, x - direction, y, swapWithOil))
                direction = -direction;
            else break;
        }
    }

    private void UpdateAcid(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;


        AcidState MoveOrDestroy(int tx, int ty)
        {
            if (tx < 0 || tx > 511 || ty < 0 || ty > 511) return AcidState.Nothing;


            if (HasType(tx, ty, swapWithAcid))
            {
                cellGrid.SwapCells(x, y, tx, ty);
                x = tx;
                y = ty;
                return AcidState.Moved;
            }

            if (HasType(tx, ty, destroyableByAcid))
            {
                Remove(x, y);
                Remove(tx, ty);
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
            if (HasType(x, y + 1, destroyableByAcid))
            {
                Remove(x, y);
                Remove(x, y + 1);
                return;
            }

            // if wasn't moved or destroyed
            break;
        }
    }

    private void UpdateGas(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        int dx = Random.Range(-1, 2);
        int dy = Random.Range(-1, 2);

        TrySwap(ref x, ref y, x + dx, y + dy, swapWithGas);
    }

    // TODO fix fire
    private void UpdateFire(Cell* cellPtr)
    {
        if (cellPtr->lifetime == 200)
        {
            Remove(cellPtr->x, cellPtr->y);
            return;
        }

        cellPtr->lifetime++;

        int x = cellPtr->x;
        int y = cellPtr->y;

        void TryFire(int tx, int ty)
        {
            if (!HasType(tx, ty, destroyableByFire)) return;
            cellGrid[ty, tx]->cellType = CellType.Fire;
        }


        // TODO gas and fire behaviour

        // control speed of firing materials
        if (cellPtr->lifetime > 50)
        {
            TryFire(x - 1, y + 1);
            TryFire(x, y + 1);
            TryFire(x + 1, y + 1);

            TryFire(x - 1, y);
            TryFire(x + 1, y);

            TryFire(x - 1, y - 1);
            TryFire(x, y - 1);
            TryFire(x + 1, y - 1);
        }

        TrySwap(ref x, ref y, x + Random.Range(-1, 2), y + 1, swapWithFire);
    }
    #endregion


    // some types of cells use loops for update
    // as here f. e. if (TrySwap(ref x, ref y, x, y - 1, swapWithSand)) { }
    // by using refs we don't need to update coords inside of if statement
    private bool TrySwap(ref int x1, ref int y1, int x2, int y2, CellType types)
    {
        if (!HasType(x2, y2, types)) return false;

        cellGrid.SwapCells(x1, y1, x2, y2);

        (x1, x2) = (x2, x1);
        (y1, y2) = (y2, y1);

        return true;
    }

    private bool HasType(int x, int y, CellType types)
    {
        if (x < 0 || x > 511 || y < 0 || y > 511) return false;
        return (cellGrid[y, x]->cellType & types) != 0;
    }

    public void Add(int x, int y, CellType cellType)
    {
        Cell cell = new Cell(x, y, cellType);
        Cell* createdPtr = cellArray.Add(cell);
        cellGrid[y, x] = createdPtr;
    }

    public void Remove(int x, int y)
    {
        Cell* toRemove = cellGrid[y, x];

        fixed (Cell* emptyPtr = &Cell.Empty)
        {
            cellGrid[y, x] = emptyPtr;

            Cell* updatePtr = cellArray.Remove(toRemove);

            if (updatePtr is null) return;
            cellGrid[updatePtr->y, updatePtr->x] = toRemove;
        }
    }
}

