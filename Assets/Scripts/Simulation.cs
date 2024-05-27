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
    private const CellType canFire = CellType.Oil | CellType.Wood;
    private const CellType canDetonate = CellType.Gas;
    private const CellType throwableByDetonation = CellType.Gas;

    private const CellType swapWithSmoke = CellType.Empty | CellType.Sand | CellType.Water | CellType.Gas | CellType.Fire | CellType.Acid | CellType.Oil;


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

                case CellType.FiringMaterial:
                    UpdateFiringMaterial(cell);
                    break;

                case CellType.Explosion:
                    UpdateExplosion(cell);
                    break;

                case CellType.Smoke:
                    UpdateSmoke(cell);
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

    private void UpdateFire(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        if (cellPtr->lifetime == 100)
        {
            if (Random.Range(0, 100) < 20)
                cellPtr->cellType = CellType.Smoke;
            else
                Remove(x, y);
            return;
        }

        cellPtr->lifetime++;



        // try to fire smth around
        for (int i = 0; i < 5; i++)
        {
            int tx = x + Random.Range(-1, 2);
            int ty = y + Random.Range(-1, 2);

            if (HasType(tx, ty, canFire))
                cellGrid[ty, tx]->cellType = CellType.FiringMaterial;
            else if (HasType(tx, ty, canDetonate))
            {
                Cell* explodedPtr = cellGrid[ty, tx];
                explodedPtr->cellType = CellType.Explosion;
            }
        }

        TrySwap(ref x, ref y, x + Random.Range(-1, 2), y + 1, swapWithFire);
    }

    private void UpdateFiringMaterial(Cell* cellPtr)
    {
        cellPtr->lifetime++;
        if (cellPtr->lifetime == 50)
        {
            cellPtr->cellType = CellType.Fire;
            return;
        }

        int x = cellPtr->x;
        int y = cellPtr->y;

        // try to spawn fire around
        for (int i = 0; i < 5; i++)
        {
            if (Random.Range(0, 100) > 5) continue;

            int tx = x + Random.Range(-1, 2);
            int ty = y + Random.Range(-1, 2);
            if (!HasType(tx, ty, CellType.Empty)) return;
            Add(tx, ty, CellType.Fire);
        }

    }

    private void UpdateExplosion(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        int generation = cellPtr->lifetime;

        //ExplodeInDirection(x, y, vx, vy);

        int dx = (int)Mathf.Sign(Random.Range(-1, 2));
        int dy = (int)Mathf.Sign(Random.Range(-1, 2));
        ExplodeInDirection(x, y, dx, dy);

        dx = (int)Mathf.Sign(Random.Range(-1, 2));
        dy = (int)Mathf.Sign(Random.Range(-1, 2));
        ExplodeInDirection(x, y, dx, dy);

        if (generation < 20) Remove(x, y);
        else cellPtr->cellType = CellType.Fire;
    }

    // x and y is coord of explosion
    private void ExplodeInDirection(int x, int y, int dx, int dy)
    {
        var tx = x + dx;
        var ty = y + dy;
        int generation = cellGrid[y, x]->lifetime;
        bool isLastGeneration = generation == 20;

        if (!isLastGeneration &&
            HasType(tx, ty, CellType.Empty))
        {
            Add(tx, ty, CellType.Explosion);
            Cell* explodedPtr = cellGrid[ty, tx];
            // first generation
            explodedPtr->lifetime = generation + 1;
        }
        if (!isLastGeneration &&
            HasType(tx, ty, throwableByDetonation) &&
            HasType(tx + dx, ty + dy, CellType.Empty))
        {
            cellGrid.SwapCells(tx, ty, tx + dx, ty + dy);
        }
    }

    private void UpdateSmoke(Cell* cellPtr)
    {
        int x = cellPtr->x;
        int y = cellPtr->y;

        for (int i = 0; i < 3; i++)
        {
            int direction = Random.Range(-1, 2);
            if (TrySwap(ref x, ref y, x + direction, y + 1, swapWithSmoke)) { }
            else if (TrySwap(ref x, ref y, x - direction, y + 1, swapWithSmoke)) { }
            else
            {
                Remove(x, y);
                break;
            }
        }
    }
    #endregion


    # region Utils Methods
    // some types of cells use loops for update
    // as here f. e. if (TrySwap(ref x, ref y, x, y - 1, swapWithSand)) { }
    // by using refs we don't need to update coords inside of if statement
    private bool TrySwap(ref int x1, ref int y1, int x2, int y2, CellType types)
    {
        if (!HasType(x2, y2, types)) return false;

        cellGrid.SwapCells(x1, y1, x2, y2);

        x1 = x2;
        y1 = y2;

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
    #endregion
}

