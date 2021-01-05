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
        List<TilesetInfo> _tmpTilesetInfos = new List<TilesetInfo>();
        SpritePacker _packer;
        struct TilesetInfo {
            public string Name;
            public int Width;
            public int Height;
        }
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
        public void AddAseTileset(string name, string ase, int tilew, int tileh)
        {
            _tmpTilesetInfos.Add(new TilesetInfo
            {
                Name = name,
                Width = tilew,
                Height = tileh
            });
            AddAseSprite(name, ase);
        }
        public void Build()
        {
            _sprites = _packer.Pack(out _texturePages);
            for(var i = 0; i < _tmpTilesetInfos.Count; i++)
            {
                var sp = _sprites[_tmpTilesetInfos[i].Name];
                _tilesets[_tmpTilesetInfos[i].Name] = new Tileset(_tmpTilesetInfos[i].Width, _tmpTilesetInfos[i].Height, sp.Texture, sp.Bounds[0]);
            }
            _tmpTilesetInfos.Clear();
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
