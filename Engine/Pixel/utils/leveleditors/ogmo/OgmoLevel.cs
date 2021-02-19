using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace kuujoo.Pixel
{
    public class OgmoLevel
    {
        public string OgmoVersion { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public OgmoLayer[] Layers { get; set; }
        public Dictionary<string, object> Values { get; set; }
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
        public List<string> GetTilesets()
        {
            var tilesets = new List<string>();
            foreach (var l in Layers)
            {
                if (l.Type == OgmoLayerType.Tile)
                {
                    if (!tilesets.Contains(l.Tileset))
                    {
                        tilesets.Add(l.Tileset);
                    }
                }
            }
            return tilesets;
        }
    }
}
