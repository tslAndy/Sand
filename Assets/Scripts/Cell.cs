public struct Cell
{
    // todo x, y to short
    // generation to byte
    // colorscale to byte
    public int x, y;
    public int vx, vy;
    public int generation;
    public int colorscale;

    public CellType cellType;

    public Cell(int x, int y, CellType cellType, int generation = 0, int colorscale = 0, int vx = 0, int vy = 0)
    {
        this.x = x;
        this.y = y;

        this.vx = vx;
        this.vy = vy;

        this.generation = generation;
        this.colorscale = colorscale;

        this.cellType = cellType;
        this.generation = generation;
    }

    public static Cell Empty = new(0, 0, CellType.Empty);
}