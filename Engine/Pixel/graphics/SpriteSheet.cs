using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public abstract class SpriteSheet : IDisposable
    {
        public string Name { get; protected set; }
        public Texture2D Texture { get; protected set; }
        Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        void Dispose(bool dispose)
        {
            if (dispose && Texture != null)
            {
                Texture.Dispose();
                _sprites.Clear();
                Texture = null;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public Sprite GetSprite(string name)
        {
            var sprite = _sprites[name];
            return sprite;
        }
        public void AddRegion(string name, Rectangle bounds, Vector2 pivot)
        {
            var sprite = new Sprite(Texture, bounds, pivot);
            _sprites[name] = sprite;
        }
    }
}
