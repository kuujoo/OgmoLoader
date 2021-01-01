using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kuujoo.Pixel
{
    public class Tile
    {
        public Rectangle Bounds { protected set; get; }
        public Texture2D Texture { protected set; get; }
        public Tile(Texture2D texture, Rectangle bounds)
        {
            Texture = texture;
            Bounds = bounds;
        }
    }
}
