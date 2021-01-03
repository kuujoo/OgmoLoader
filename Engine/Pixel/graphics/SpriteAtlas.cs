using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public abstract class SpriteAtlas : IDisposable
    {
        public string Name { get; protected set; }
        public Texture2D Texture { get; protected set; }
        Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        Dictionary<string, Tileset> _tilesets = new Dictionary<string, Tileset>();
        void Dispose(bool dispose)
        {
            if (dispose && Texture != null)
            {
                _sprites.Clear();
                _tilesets.Clear();
                Texture.Dispose();
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
        public Tileset GetTileset(string name)
        {
            var tileset = _tilesets[name];
            return tileset;
        }
        public void AddSpriteRegion(string name, Rectangle bounds, Vector2 pivot)
        {
            var sprite = new Sprite(Texture, bounds, pivot);
            _sprites[name] = sprite;
        }
        public void AddTilesetRegion(string name, Rectangle bounds, int width, int height)
        {
            _tilesets[name] = new Tileset(width, height, Texture, bounds);
        }
    }
}
