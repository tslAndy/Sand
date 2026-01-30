using MemoryPack;

public class SaveSystem
{
    public static T? Load<T>(string path) where T : class
    {
        if (!File.Exists(path))
            return null;

        byte[] bytes = File.ReadAllBytes(path);
        T? data = MemoryPackSerializer.Deserialize<T>(bytes);
        return data;
    }

    public static void Save<T>(T data, string path) where T: class
    {
        byte[] bytes = MemoryPackSerializer.Serialize(data);
        File.WriteAllBytes(path, bytes);
    }
}
