using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kuujoo.Pixel
{
    public class Sprite
    {
        public Texture2D Texture { get; protected set; }
        public Rectangle Bounds { get; protected set; }
        public Vector2 Pivot { get; protected set; }
        public Sprite(Texture2D texture, Rectangle bounds)
        {
            Texture = texture;
            Bounds = bounds;
            Pivot = new Vector2(0.0f, 0.0f);
        }
        public Sprite(Texture2D texture, Rectangle bounds, Vector2 pivot)
        {
            Texture = texture;
            Bounds = bounds;
            Pivot = pivot;
        }
    }
}
