using System.IO;
using System.Text.Json;

namespace kuujoo.Pixel
{
    public class LdtkWorld
    {
        public int WorldGridWidth { get; set; }
        public int WorldGridHeight { get; set; }
        public LdtkDefines Defs { get; set; }
        public LdtkLevel[] Levels {get; set; }
        public string DefaultLevelBgColor { get; set; }
        public static LdtkWorld Load(string file)
        {
            var data = File.ReadAllText(file);
            var options = new JsonSerializerOptions
            {
               PropertyNameCaseInsensitive = true,
               IgnoreNullValues = true
            };
            var result = JsonSerializer.Deserialize<LdtkWorld>(data, options);
            return result;
        }
    }
}
