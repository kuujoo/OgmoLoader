using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class OgmoLevel : OgmoValueContainer
    {
        public string OgmoVersion { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public OgmoLayer[] Layers { get; set; }
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
