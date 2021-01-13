using Microsoft.Xna.Framework;
namespace kuujoo.Pixel
{
    public class AseCel : IAseUserData
    {
        public string UserDataText { get; set; }
        public Color UserDataColor { get; set; }
        public AseLayer Layer;
        public Color[] Pixels;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public float Alpha;
    }
}