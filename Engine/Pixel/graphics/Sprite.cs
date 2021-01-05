using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace kuujoo.Pixel
{
    public class Sprite
    {
        public Texture2D Texture { get; protected set; }
        public Rectangle[] Bounds => _bounds;
        public Vector2 Pivot { get; protected set; }
        public string Tag { get; protected set; }
        Rectangle[] _bounds;
        public Sprite(Texture2D texture, Rectangle[] bounds, string tag)
        {
            Texture = texture;
            _bounds = bounds;
            Pivot = Vector2.Zero;
            Tag = tag;
        }
        public Sprite(Texture2D texture, Rectangle[] bounds, Vector2 pivot, string tag)
        {
            Texture = texture;
            _bounds = bounds;
            Pivot = pivot;
            Tag = tag;
        }
    }
}
