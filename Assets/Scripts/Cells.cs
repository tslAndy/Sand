public struct Cell
{
    public int x, y;
    public int vx, vy;
    public byte generation;
    public byte grayscale;

    public CellType cellType;

    public Cell(int x, int y, CellType cellType, int vx = 0, int vy = 0, byte generation = 0, byte grayscale = 0)
    {
        this.x = x;
        this.y = y;

        this.vx = vx;
        this.vy = vy;

        this.generation = generation;
        this.grayscale = grayscale;

        this.cellType = cellType;
        this.generation = generation;
    }

    public static Cell Empty = new(0, 0, CellType.Empty);
}