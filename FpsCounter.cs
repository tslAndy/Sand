using System.Diagnostics;

public static class FpsCounter
{
    private static Stopwatch sw = new();
    private static string[] nums = new string[300];

    private static string fps = "";
    private static float time = 0f;
    private static float dt = 0f;

    static FpsCounter()
    {
        for (int i = 0; i < 300; i++)
            nums[i] = i.ToString();
    }

    public static string FPS => fps;
    public static float DT => dt;

    public static void Start() => sw.Restart();

    public static void Stop()
    {
        sw.Stop();
        dt = sw.ElapsedMilliseconds * 0.001f;
        time += dt;

        if (time > 0.5f)
        {
            time = 0f;
            fps = nums[Math.Clamp(1000 / sw.ElapsedMilliseconds, 0, 299)];
        }
    }
}
