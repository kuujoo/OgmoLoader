using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace kuujoo.Pixel
{
    public class SpriteResources : SceneComponent
    {
        public Texture2D[] TexturePages => _texturePages;
        Texture2D[] _texturePages;
        Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        Dictionary<string, Tileset> _tilesets = new Dictionary<string, Tileset>();
        SpritePacker _packer;
        public SpriteResources(int texturepagewidth, int texturepageheight)
        {
            _packer = new SpritePacker(texturepagewidth, texturepageheight);
        }
        public void AddAseSprite(string name, string ase)
        {
            var s = new AseSprite(ase, false);
            for (var i = 0; i < s.FrameCount; i++)
            {
                _packer.Add(name, s.Width, s.Height, i, s.Frames[i].Pixels);
            }
        }
        public void InitTileset(string spritename, int tilewidth, int tileheight)
        {
            var sprite = GetSprite(spritename);
            _tilesets[spritename] = new Tileset(12, 12, sprite.Texture, sprite.Bounds[0]);
        }
        public void Build()
        {
            _sprites = _packer.Pack(out _texturePages);
        }
        public Sprite GetSprite(string sprite)
        {
            return _sprites[sprite];
        }
        public Tileset GetTileset(string tileset)
        {
            return _tilesets[tileset];
        }
        public override void CleanUp()
        {
            _sprites.Clear();
            _tilesets.Clear();
            for (var i = 0; i< _texturePages.Length; i++)
            {
                _texturePages[i].Dispose();
            }
        }
    }
}
