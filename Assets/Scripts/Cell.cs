public struct Cell
{
    public int x, y;
    public int generation;
    public int grayscale;

    public CellType cellType;

    public Cell(int x, int y, CellType cellType, int generation = 0, byte grayscale = 0)
    {
        this.x = x;
        this.y = y;

        this.generation = generation;
        this.grayscale = grayscale;

        this.cellType = cellType;
        this.generation = generation;
    }

    public static Cell Empty = new(0, 0, CellType.Empty);
}