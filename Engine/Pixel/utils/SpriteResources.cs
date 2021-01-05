using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace kuujoo.Pixel
{
    public class SpriteResources : SceneComponent
    {
        public Texture2D[] TexturePages => _texturePages;
        Texture2D[] _texturePages;
        Dictionary<string, List<Sprite>> _sprites = new Dictionary<string, List<Sprite>>();
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
            if (s.Tags.Count == 0)
            {
                SpritePacker.PixelRect rect = new SpritePacker.PixelRect(name, "", s.FrameCount, s.FrameCount * s.Width, s.Height);
                for (var i = 0; i < s.FrameCount; i++)
                {
                    for (var j = 0; j < s.Height; j++)
                    {
                        var ii = i * s.Width + j * rect.Width;
                        var kk = j * s.Width;
                        Array.Copy(s.Frames[i].Pixels, kk, rect.Pixels, ii, s.Width);
                    }
                }
                _packer.Add(rect);
            }
            else
            {
                for (var t = 0; t < s.Tags.Count; t++)
                {
                    var frames = s.Tags[t].To - s.Tags[t].From + 1;
                    SpritePacker.PixelRect rect = new SpritePacker.PixelRect(name, s.Tags[t].Name, frames, frames * s.Width, s.Height);
                    for (var i = s.Tags[t].From; i <= s.Tags[t].To; i++)
                    {
                        for (var j = 0; j < s.Height; j++)
                        {
                            var ii = i * s.Width + j * rect.Width;
                            var kk = j * s.Width;
                            Array.Copy(s.Frames[i].Pixels, kk, rect.Pixels, ii, s.Width);
                        }
                    }
                    _packer.Add(rect);
                }
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
                _tilesets[_tmpTilesetInfos[i].Name] = new Tileset(_tmpTilesetInfos[i].Width, _tmpTilesetInfos[i].Height, sp[0].Texture, sp[0].Bounds[0]);
            }
            _tmpTilesetInfos.Clear();
        }
        public Sprite GetSprite(string sprite)
        {
            return _sprites[sprite][0];
        }
        public Sprite GetSprite(string sprite, string tag)
        {
            var sprites = _sprites[sprite];
            for(var i = 0; i < sprites.Count; i++)
            {
                if(sprites[i].Tag == tag)
                {
                    return sprites[i];
                }
            }
            return null;
        }
        public Tileset GetTileset(string tileset)
        {
            return _tilesets[tileset];
        }
        public override void CleanUp()
        {
            _sprites.Clear();
            _tilesets.Clear();
            if (_texturePages != null)
            {
                for (var i = 0; i < _texturePages.Length; i++)
                {
                    _texturePages[i].Dispose();
                }
            }
            _texturePages = null;
        }
    }
}
