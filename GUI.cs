using System.Numerics;
using Raylib_cs;

public static class GUI
{
    private static List<ButtonData> buttons = new();
    private static int pressedIndex = -1;
    private static int selectedIndex = -1;
    private static bool pressedThisFrame = false;

    private static Color SELECTED_COLOR = Color.Orange;
    private static Color PRESSED_COLOR = Color.Red;

    public static int AddButton(ButtonData data)
    {
        buttons.Add(data);
        return buttons.Count - 1;
    }

    public static void Update()
    {
        pressedThisFrame = false;

        for (int i = 0; i < buttons.Count; i++)
        {
            ButtonData tempData = buttons[i];
            Raylib.DrawRectangle(tempData.x, tempData.y, tempData.w, tempData.h, tempData.backCol);
            Raylib.DrawText(tempData.text, tempData.x + 2, tempData.y, tempData.fontSize, tempData.fontCol);
        }

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
            CheckPressed();
        else
            CheckSelected();

        DrawPressed();
        DrawSelected();
    }

    public static bool TryGetPressedId(out int id)
    {
        if (pressedThisFrame)
        {
            id = buttons[pressedIndex].id;
            return true;
        }
        id = -1;
        return false;
    }

    private static void CheckSelected()
    {
        Vector2 pos = Raylib.GetMousePosition();
        int x = (int)pos.X;
        int y = (int)pos.Y;
        for (int i = 0; i < buttons.Count; i++)
        {
            ButtonData data = buttons[i];
            if (data.x < x && x < data.x + data.w && data.y < y && y < data.y + data.h)
            {
                selectedIndex = i;
                return;
            }
        }
        selectedIndex = -1;
    }

    private static void CheckPressed()
    {
        Vector2 pos = Raylib.GetMousePosition();
        int x = (int)pos.X;
        int y = (int)pos.Y;
        for (int i = 0; i < buttons.Count; i++)
        {
            ButtonData data = buttons[i];
            if (!(data.x < x && x < data.x + data.w && data.y < y && y < data.y + data.h))
                continue;

            pressedIndex = i;
            pressedThisFrame = true;
            return;
        }
    }

    private static void DrawSelected()
    {
        if (selectedIndex < 0)
            return;

        ButtonData data = buttons[selectedIndex];
        Rectangle rect = new Rectangle
        {
            X = data.x - 4,
            Y = data.y - 4,
            Width = data.w + 4,
            Height = data.h + 4
        };
        Raylib.DrawRectangleLinesEx(rect, 4, SELECTED_COLOR);
    }

    private static void DrawPressed()
    {
        if (pressedIndex < 0)
            return;

        ButtonData data = buttons[pressedIndex];
        Rectangle rect = new Rectangle
        {
            X = data.x - 4,
            Y = data.y - 4,
            Width = data.w + 4,
            Height = data.h + 4
        };
        Raylib.DrawRectangleLinesEx(rect, 4, PRESSED_COLOR);
    }
}

public class ButtonData
{
    public int x, y;
    public int w, h;
    public int id;
    public Color backCol, fontCol;
    public int fontSize;
    public string text;
}
