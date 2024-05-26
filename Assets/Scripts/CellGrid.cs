public unsafe class CellGrid
{
    private Cell*[] cells = new Cell*[262_144];

    public CellGrid()
    {
        fixed (Cell* emptyPtr = &Cell.Empty)
        {
            for (int y = 0; y < 512; y++)
                for (int x = 0; x < 512; x++)
                    this[y, x] = emptyPtr;
        }
    }

    public Cell* this[int y, int x]
    {
        get => cells[(y << 9) + x];
        set => cells[(y << 9) + x] = value;
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
}