namespace kuujoo.Pixel
{
    public class OgmoEntity : OgmoValueContainer
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Rotation { get; set; }
        public int Width { get; set; }
        public int OriginX { get; set; }
        public int OriginY { get; set; }
        public bool FlippedX { get; set; }
        public bool FlippedY { get; set; }
    }
}
