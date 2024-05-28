using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public unsafe class Simulation : MonoBehaviour
{
    public CellArray cellArray = new();
    public CellGrid cellGrid = new();

    // TODO random position with random between -1 and 1

    // some types of cells use loops for update
    // as here f. e. if (TrySwap(ref x, ref y, x, y - 1, swapWithSand)) { }
    // by using refs we don't need to update coords inside of if statement
    public bool TrySwap(ref int x1, ref int y1, int x2, int y2, CellType types)
    {
        if (!HasType(x2, y2, types)) return false;

        cellGrid.SwapCells(x1, y1, x2, y2);

        x1 = x2;
        y1 = y2;

        return true;
    }

    public bool HasType(int x, int y, CellType types)
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

