namespace kuujoo.Pixel
{
    public enum OgmoLayerType
    {
        Tile,
        Entity,
        Grid
    }
    public class OgmoLayer
    {
        public OgmoLayerType Type
        {
            get
            {
                if (Data != null)
                {
                    return OgmoLayerType.Tile;
                }
                else
                {
                    return OgmoLayerType.Entity;
                }
            }
        }
        public string Name { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int GridCellWidth { get; set; }
        public int GridCellHeight { get; set; }
        public int GridCellsX { get; set; }
        public int GridCellsY { get; set; }
        public string Tileset { get; set; }
        public int[] Data { get; set; }
        public OgmoEntity[] Entities { get; set; }
        public int ExportMode { get; set; }
        public int ArrayMode { get; set; }
    }
}
