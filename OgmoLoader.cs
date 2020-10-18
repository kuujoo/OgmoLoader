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
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Deserialize<OgmoLevel>(data, options);
        }
    }
}
