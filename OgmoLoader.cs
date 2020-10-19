using System;
using System.IO;
using System.Text.Json;

namespace kuujoo.Pixel
{
    public static class OgmoLoader
    {
        public static OgmoLevel LoadLevel(string path)
        {
            var data = File.ReadAllText(path);
            if (data.Length == 0) return null;
            return LoadFromData(data);
        }
        public static OgmoLevel LoadFromData(string data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Deserialize<OgmoLevel>(data, options);
        }
        public static OgmoWorld LoadWorld(string path)
        {
            var data = File.ReadAllText(path);
            if (data.Length == 0) return null;
            return JsonSerializer.Deserialize<OgmoWorld>(data);
        }
        public static void SaveWorld(string file, OgmoWorld world)
        {
            var data = JsonSerializer.Serialize<OgmoWorld>(world);
            File.WriteAllText(file, data);
        }
    }
}
