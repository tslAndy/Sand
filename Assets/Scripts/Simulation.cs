using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public unsafe class Simulation : MonoBehaviour
{
    private Cell*[] cellsGrid = new Cell*[262_144];
    private Cell[] cellsArray = new Cell[200_000];
    private int firstInactiveIndex;
    public int Length => firstInactiveIndex;

    private void Awake()
    {
        fixed (Cell* emptyPtr = &Cell.Empty)
        {
            for (int y = 0; y < 512; y++)
                for (int x = 0; x < 512; x++)
                    this[y, x] = emptyPtr;
        }
    }

    // some types of cells use loops for update
    // as here f. e. if (TrySwap(ref x, ref y, x, y - 1, swapWithSand)) { }
    // by using refs we don't need to update coords inside of if statement
    public bool TrySwap(ref int x1, ref int y1, int x2, int y2, CellType types)
    {
        if (!HasType(x2, y2, types)) return false;

        SwapCells(x1, y1, x2, y2);

        x1 = x2;
        y1 = y2;

        return true;
    }

    public bool HasType(int x, int y, CellType types)
    {
        if (x < 0 || x > 511 || y < 0 || y > 511) return false;
        return (this[y, x]->cellType & types) != 0;
    }

    public bool HasType(int x, int y, CellType types, out Cell* cellPtr)
    {
        cellPtr = null;
        if (x < 0 || x > 511 || y < 0 || y > 511) return false;
        Cell* testPtr = this[y, x];
        if ((testPtr->cellType & types) == 0) return false;
        cellPtr = testPtr;
        return true;
    }

    public void ChangeCellType(Cell* cellPtr, CellType cellType, int generation = 0)
    {
        cellPtr->cellType = cellType;
        cellPtr->generation = generation;
    }

    public void Add(int x, int y, CellType cellType, int generation = 0)
    {
        Cell cell = new Cell(x, y, cellType, generation);
        Cell* createdPtr = AddToArray(cell);
        this[y, x] = createdPtr;
    }

    public void TryAdd(int x, int y, CellType cellType, int generation = 0)
    {
        if (!HasType(x, y, cellType)) return;
        Add(x, y, cellType, generation);
    }

    public void Remove(int x, int y)
    {
        Cell* toRemove = this[y, x];

        fixed (Cell* emptyPtr = &Cell.Empty)
        {
            this[y, x] = emptyPtr;

            Cell* updatePtr = RemoveFromArray(toRemove);

            if (updatePtr is null) return;
            this[updatePtr->y, updatePtr->x] = toRemove;
        }
    }

    private Cell* AddToArray(Cell cell)
    {
        cellsArray[firstInactiveIndex] = cell;
        fixed (Cell* ptr = &cellsArray[firstInactiveIndex])
        {
            firstInactiveIndex++;
            return ptr;
        }
    }

    private Cell* RemoveFromArray(Cell* cell)
    {
        fixed (Cell* cellsPtr = cellsArray)
        {
            firstInactiveIndex--;
            long indexToRemove = cell - cellsPtr;

            // if deleted cell was last active do nothing
            // because no one cell will be on it place
            if (firstInactiveIndex == indexToRemove) return null;

            // set value of last cell instead of deleted
            cellsArray[indexToRemove] = cellsArray[firstInactiveIndex];

            // return moved cell pointer to update it in the grid
            // now cell at position of firstInactive should contain *cell
            return cellsPtr + firstInactiveIndex;
        }
    }

    public void SwapCells(int x1, int y1, int x2, int y2)
    {
        Cell* cell1 = this[y1, x1];
        Cell* cell2 = this[y2, x2];

        cell1->x = x2;
        cell1->y = y2;

        cell2->x = x1;
        cell2->y = y1;

        this[y2, x2] = cell1;
        this[y1, x1] = cell2;
    }

    public Cell* this[int index]
    {
        get
        {
            fixed (Cell* c = &cellsArray[index])
                return c;
        }
    }

    public Cell* this[int y, int x]
    {
        get => cellsGrid[(y << 9) + x];
        set => cellsGrid[(y << 9) + x] = value;
    }
}

