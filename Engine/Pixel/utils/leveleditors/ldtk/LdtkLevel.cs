namespace kuujoo.Pixel
{
    public class LdtkLevel
    {
        public string Identifier { get; set; }
        public int Uid { get; set; }
        public int WorldX { get; set; }
        public int WorldY { get; set; }
        public int PxWid { get; set; }
        public int PxHei { get; set; }
        public string BgColor { get; set; }
        public LdtkLayerInstance[] LayerInstances { get; set; }
    }
}
