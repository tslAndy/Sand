using System.Numerics;
using Raylib_cs;

public class Program
{
    private const int SCREEN_WIDTH = 1224;
    private const int SCREEN_HEIGHT = 1024;

    private static Game game = new();
    private static PType brush;

    public static void Main(string[] args)
    {
        Raylib.InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Hello world");
        Texture2D texture = Raylib.LoadTexture(Path.Combine(Directory.GetCurrentDirectory(), "Display.png"));

        InitUI();

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            GUI.Update();
            CheckUI();
            CheckInput();

            FpsCounter.Start();
            game.Update();
            FpsCounter.Stop();

            game.Render();

            Raylib.UpdateTexture(texture, game.colors);
            Raylib.DrawTextureEx(texture, new Vector2(1023, 1023), 180, 1, Color.White);
            // DrawActivity();

            Raylib.DrawText(FpsCounter.FPS, 20, 20, 30, Color.Green);
            Raylib.EndDrawing();
        }

        Raylib.UnloadTexture(texture);
        Raylib.CloseWindow();
    }

    private static void InitUI()
    {
        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20,
            w = 60,
            h = 20,
            id = (int)PType.Empty,
            backCol = Color.Gray,
            fontCol = Color.White,
            fontSize = 20,
            text = "EMPT"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 30,
            w = 60,
            h = 20,
            id = (int)PType.Wall,
            backCol = Color.DarkGray,
            fontCol = Color.Black,
            fontSize = 20,
            text = "WALL"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 60,
            w = 60,
            h = 20,
            id = (int)PType.Sand,
            backCol = Color.Yellow,
            fontCol = Color.Black,
            fontSize = 20,
            text = "SAND"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 90,
            w = 60,
            h = 20,
            id = (int)PType.Water,
            backCol = Color.Blue,
            fontCol = Color.White,
            fontSize = 20,
            text = "WATR"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 120,
            w = 60,
            h = 20,
            id = (int)PType.Oil,
            backCol = Color.DarkGreen,
            fontCol = Color.White,
            fontSize = 20,
            text = "OIL"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 150,
            w = 60,
            h = 20,
            id = (int)PType.Acid,
            backCol = Color.Lime,
            fontCol = Color.Black,
            fontSize = 20,
            text = "ACID"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 180,
            w = 60,
            h = 20,
            id = (int)PType.Ice,
            backCol = Color.SkyBlue,
            fontCol = Color.Black,
            fontSize = 20,
            text = "ICE"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 210,
            w = 60,
            h = 20,
            id = (int)PType.Stone,
            backCol = Color.Gray,
            fontCol = Color.White,
            fontSize = 20,
            text = "STON"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 240,
            w = 60,
            h = 20,
            id = (int)PType.Wood,
            backCol = Color.Brown,
            fontCol = Color.White,
            fontSize = 20,
            text = "WOOD"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 270,
            w = 60,
            h = 20,
            id = (int)PType.Gas,
            backCol = Color.Purple,
            fontCol = Color.Black,
            fontSize = 20,
            text = "GAS"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 300,
            w = 60,
            h = 20,
            id = (int)PType.Cloner,
            backCol = Color.DarkPurple,
            fontCol = Color.Black,
            fontSize = 20,
            text = "CLON"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 330,
            w = 60,
            h = 20,
            id = (int)PType.Smoke,
            backCol = Color.LightGray,
            fontCol = Color.Black,
            fontSize = 20,
            text = "SMOK"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 360,
            w = 60,
            h = 20,
            id = (int)PType.Fire,
            backCol = Color.Orange,
            fontCol = Color.Black,
            fontSize = 20,
            text = "FIRE"
        });

        GUI.AddButton(new ButtonData()
        {
            x = 1044,
            y = 20 + 390,
            w = 60,
            h = 20,
            id = (int)PType.Lava,
            backCol = new Color(252, 57, 3),
            fontCol = Color.Black,
            fontSize = 20,
            text = "LAVA"
        });
    }

    private static void DrawActivity()
    {
        for (int y = 0; y < 1024; y += 32)
        {
            for (int x = 0; x < 1024; x += 32)
            {
                if (!game.activity[1023 - x, 1023 - y])
                    continue;

                Raylib.DrawRectangleLines(x, y, 32, 32, Color.Green);
            }
        }
    }

    private static void CheckUI()
    {
        if (GUI.TryGetPressedId(out int id))
            brush = (PType)id;
    }

    private static void CheckInput()
    {
        if (!Raylib.IsMouseButtonDown(MouseButton.Left))
            return;

        Vector2 pos = Raylib.GetMousePosition();
        int x = 1023 - (int)pos.X;
        int y = 1023 - (int)pos.Y;

        if (x < 0 || y < 0)
            return;

        int rad = 10;
        int startX = Math.Max(x - rad, 1);
        int endX = Math.Min(x + rad, 1023);
        int startY = Math.Max(y - rad, 1);
        int endY = Math.Min(y + rad, 1023);

        for (int ty = startY; ty < endY; ty++)
        {
            for (int tx = startX; tx < endX; tx++)
            {
                int dx = x - tx;
                int dy = y - ty;
                if (dx * dx + dy * dy > rad * rad)
                    continue;
                game[tx, ty] = new Point { type = brush };
            }
        }
    }
}
