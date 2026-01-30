using JobSystem;
using Raylib_cs;

public class Game
{
    public Color[] colors = new Color[1024 * 1024];
    public Activity activity = new();
    public Extents extents = new();

    private Point[] _field;
    private Logic _logic;

    private bool forward;

    public Game()
    {
        Point[]? temp = SaveSystem.Load<Point[]>("./save");
        if (temp != null)
        {
            _field = temp;
            _logic = new Logic(_field, extents);
            return;
        }

        _field = new Point[1024 * 1024];
        for (int x = 0; x < 1024; x++)
        {
            this[x, 0] = new Point { type = PType.Wall };
            this[x, 1023] = new Point { type = PType.Wall };
        }

        for (int y = 0; y < 1024; y++)
        {
            this[0, y] = new Point { type = PType.Wall };
            this[1023, y] = new Point { type = PType.Wall };
        }            
        
        _logic = new Logic(_field, extents);
    }

    public void Update()
    {
        for (int i = 0; i < _field.Length; i++)
            _field[i].isUpdated = false;

        forward = !forward;

        JobHandle handle = default;
        for (int y = 0; y < 1024; y += 64)
        {
            ChunkJob jobA = new ChunkJob
            {
                activity = activity,
                extents = extents,
                logic = _logic,
                offsetY = y,
                baseOffsetX = 0,
                forward = forward
            };

            ChunkJob jobB = new ChunkJob
            {
                activity = activity,
                extents = extents,
                logic = _logic,
                offsetY = y,
                baseOffsetX = 64,
                forward = forward
            };

            handle = jobA.Schedule(8, 4, handle);
            handle = jobB.Schedule(8, 4, handle);
        }

        JobManager.RunJobs();
    }

    public void Render()
    {
        for (int i = 0; i < _field.Length; i++)
        {
            colors[i] = _field[i].type switch
            {
                PType.Empty => Color.Black,
                PType.Wall => Color.DarkGray,
                PType.Sand => Color.Yellow,
                PType.Water => Color.Blue,
                PType.Oil => Color.DarkGreen,
                PType.Acid => Color.Lime,
                PType.Ice => Color.SkyBlue,
                PType.Stone => Color.Gray,
                PType.Wood => Color.Brown,
                PType.Gas => Color.Purple,
                PType.Cloner => Color.DarkPurple,
                PType.Smoke => Color.LightGray,
                PType.Fire => Color.Orange,
                PType.Ignite => Color.Red,
                PType.Lava => new Color(252, 57, 3),
                _ => Color.Black
            };
        }
    }

    public void Save()
    {
        SaveSystem.Save(_field, "./save");
    }
    
    public Point this[int x, int y]
    {
        get => _field[y * 1024 + x];
        set
        {
            _field[y * 1024 + x] = value;
            activity[x, y] = true;
            extents.Push(x, y);
        }
    }
}
