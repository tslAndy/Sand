using System;
using UnityEngine;

public unsafe class CellArray
{
    private Cell[] cells = new Cell[200_000];
    private int firstInactiveIndex;
    public int Length => firstInactiveIndex;

    public Cell* Add(Cell cell)
    {
        cells[firstInactiveIndex] = cell;
        fixed (Cell* ptr = &cells[firstInactiveIndex])
        {
            firstInactiveIndex++;
            return ptr;
        }
    }

    public Cell* Remove(Cell* cell)
    {
        fixed (Cell* cellsPtr = cells)
        {
            firstInactiveIndex--;
            long indexToRemove = cell - cellsPtr;

            // if deleted cell was last active do nothing
            // because no one cell will be on it place
            if (firstInactiveIndex == indexToRemove) return null;

            // set value of last cell instead of deleted
            cells[indexToRemove] = cells[firstInactiveIndex];

            // return moved cell pointer to update it in the grid
            // now cell at position of firstInactive should contain *cell
            return cellsPtr + firstInactiveIndex;
        }
    }

    public Cell* this[int i]
    {
        get
        {
            fixed (Cell* c = &cells[i])
            {
                return c;
            }
        }
    }
}


public struct Cell
{
    public int x, y;
    public CellType cellType;
    public int lifetime;

    public Cell(int x, int y, CellType cellType, int lifetime = 0)
    {
        this.x = x;
        this.y = y;

        this.cellType = cellType;
        this.lifetime = lifetime;
    }

    public static Cell Empty = new(0, 0, CellType.Empty);
}

[Serializable]
public enum CellType
{
    Empty = 1,
    Sand = 2,
    Water = 4,
    Gas = 8,
    Fire = 16,
    Stone = 32,
    Acid = 64,
    Oil = 128,
    Wood = 256,
    FiringMaterial = 512,
    Explosion = 1024,
    Smoke = 2048
}

public enum AcidState
{
    Nothing,
    Moved,
    Destroyed
}