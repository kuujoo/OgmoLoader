using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public class AseSlice : IAseUserData
    {
        public string UserDataText { get; set; }
        public Color UserDataColor { get; set; }
        public int Frame;
        public string Name;
        public int OriginX;
        public int OriginY;
        public int Width;
        public int Height;
        public Vector2? Pivot;
    }
}