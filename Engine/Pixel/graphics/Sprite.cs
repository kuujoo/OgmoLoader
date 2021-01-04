using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kuujoo.Pixel
{
    public class Sprite
    {
        public Texture2D Texture { get; protected set; }
        public Rectangle[] Bounds { get; protected set; }
        public Vector2 Pivot { get; protected set; }
        public Sprite(Texture2D texture, Rectangle bounds)
        {
            Texture = texture;
            Bounds = new Rectangle[1];
            Bounds[0] = bounds;
            Pivot = Vector2.Zero;

        }
        public Sprite(Texture2D texture, Rectangle bounds, Vector2 pivot)
        {
            Texture = texture;
            Bounds = new Rectangle[1];
            Bounds[0] = bounds;
            Pivot = pivot;
        }
        public Sprite(Texture2D texture, Rectangle[] bounds)
        {
            Texture = texture;
            Bounds = bounds;
            Pivot = Vector2.Zero;
        }
        public Sprite(Texture2D texture, Rectangle[] bounds, Vector2 pivot)
        {
            Texture = texture;
            Bounds = bounds;
            Pivot = pivot;
        }
    }
}
