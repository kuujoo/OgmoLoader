using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace kuujoo.Pixel
{
    public class SpriteResources : SceneComponent
    {
        public TexturePage[] TexturePages => _texturePages;
        TexturePage[] _texturePages;
        Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>(); 
        SpritePacker _packer;
        string _spritedirectory;
        struct SpriteInfo
        {
            public string Name;
            public AseSprite AseSPrite;
            public int PackId;
        }
        public SpriteResources(int texturepagewidth, int texturepageheight, string spritedirectory)
        {
            _packer = new SpritePacker(texturepagewidth, texturepageheight);
            _spritedirectory = spritedirectory;
            BuildSprites();
        }
        void BuildSprites()
        {
            BuildAseSprites();
        }
        void BuildAseSprites()
        {
            var files = Directory.GetFiles(_spritedirectory, "*.ase");
            int packid = 0;
            List<SpriteInfo> spriteInfo = new List<SpriteInfo>(files.Length);
            for (var i = 0; i < files.Length; i++)
            {
                spriteInfo.Add(new SpriteInfo()
                {
                    AseSPrite = new AseSprite(files[i]),
                    Name = Path.GetFileNameWithoutExtension(files[i]),
                    PackId = packid

                });
                for (var j = 0; j < spriteInfo[i].AseSPrite.FrameCount; j++)
                {
                    _packer.Add(packid, spriteInfo[i].AseSPrite.Width, spriteInfo[i].AseSPrite.Height, spriteInfo[i].AseSPrite.Frames[j].Pixels);
                    packid++;
                }
            }

            _texturePages = _packer.Pack();

            for (var i = 0; i < spriteInfo.Count; i++)
            {
                var sprite = new Sprite();
                if (spriteInfo[i].AseSPrite.Slices.Count > 0 && spriteInfo[i].AseSPrite.Slices[0].Pivot.HasValue)
                {
                    var pivot = spriteInfo[i].AseSPrite.Slices[0].Pivot.Value;
                    sprite.SetPivot(pivot);
                }
                if (spriteInfo[i].AseSPrite.Tags.Count > 0)
                {
                    for (var t = 0; t < spriteInfo[i].AseSPrite.Tags.Count; t++)
                    {
                        var animation = new Sprite.Animation()
                        {
                            Name = spriteInfo[i].AseSPrite.Tags[t].Name
                        };
                        for (int f = spriteInfo[i].AseSPrite.Tags[t].From; f <= spriteInfo[i].AseSPrite.Tags[t].To; f++)
                        {
                            for (var p = 0; p < _texturePages.Length; p++)
                            {
                                Rectangle rect;
                                if (_texturePages[p].SubTextures.TryGetValue(spriteInfo[i].PackId + f, out rect))
                                {
                                    var frame = new Sprite.Frame()
                                    {
                                        Texture = _texturePages[p].Texture,
                                        Rect = rect,
                                        Duration = spriteInfo[i].AseSPrite.Frames[f].Duration / 1000.0f
                                    };
                                    animation.Frames.Add(frame);
                                }
                            }
                        }
                        sprite.Animations.Add(animation);
                    }
                    _sprites[spriteInfo[i].Name] = sprite;
                }
                else
                {
                    var animation = new Sprite.Animation()
                    {
                        Name = "animation"
                    };
                    for (int f = 0; f < spriteInfo[i].AseSPrite.Frames.Count; f++)
                    {
                        for (var p = 0; p < _texturePages.Length; p++)
                        {
                            Rectangle rect;
                            if (_texturePages[p].SubTextures.TryGetValue(spriteInfo[i].PackId + f, out rect))
                            {
                                var frame = new Sprite.Frame()
                                {
                                    Texture = _texturePages[p].Texture,
                                    Rect = rect,
                                    Duration = spriteInfo[i].AseSPrite.Frames[f].Duration / 1000.0f
                                };
                                animation.Frames.Add(frame);
                                break;
                            }
                        }
                    }
                    sprite.Animations.Add(animation);
                    _sprites[spriteInfo[i].Name] = sprite;
                }
            }
        }
        public Sprite GetSprite(string sprite)
        {
            return _sprites[sprite];
        }
        public override void CleanUp()
        {
            _sprites.Clear();
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
