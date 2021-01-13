using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using kuujoo.Pixel.StbImageSharp;

namespace kuujoo.Pixel
{
    public class SpriteResources : SceneComponent
    {
        public TexturePage[] TexturePages => _texturePages;
        TexturePage[] _texturePages;
        Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>(); 
        SpritePacker _packer;
        string _spritedirectory;
        static string[] _wildcards = { "*.png", "*.jpg", "*.jpeg", "*.bmp" };
        struct SpriteInfo
        {
            public string Name;
            public AseSprite AseSprite;
            public int PackId;
            public int Width;
            public int Height;
        }
        public SpriteResources(int texturepagewidth, int texturepageheight, string spritedirectory)
        {
            _packer = new SpritePacker(texturepagewidth, texturepageheight);
            _spritedirectory = spritedirectory;
            BuildSprites();
        }
        void BuildSprites()
        {
            int packid = 0;
            List<SpriteInfo> spriteInfo = new List<SpriteInfo>(20);
            { // Ase load
                var files = Directory.GetFiles(_spritedirectory, "*.ase");
                for (var i = 0; i < files.Length; i++)
                {
                    var ase = new AseSprite(files[i]);
                    spriteInfo.Add(new SpriteInfo()
                    {
                        AseSprite = ase,
                        Name = Path.GetFileNameWithoutExtension(files[i]),
                        PackId = packid,
                        Width = ase.Width,
                        Height = ase.Height

                    });
                    for (var j = 0; j < spriteInfo[i].AseSprite.FrameCount; j++)
                    {
                        _packer.Add(packid, ase.Width, ase.Height, spriteInfo[i].AseSprite.Frames[j].Pixels);
                        packid++;
                    }
                }
            }
            { // PNG load
                for (var w = 0; w < _wildcards.Length; w++)
                {
                    var files = Directory.GetFiles(_spritedirectory, _wildcards[w] );
                    if (files.Length > 0)
                    {
                        for (var i = 0; i < files.Length; i++)
                        {
                            using (var stream = File.OpenRead(files[i]))
                            {
                                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                                Color[] pixels = new Color[image.Width * image.Height];
                                for (var ii = 0; ii < image.Width * image.Height; ii++)
                                {
                                    pixels[ii] = new Color(image.Data[ii * 4 + 0], image.Data[ii * 4 + 1], image.Data[ii * 4 + 2], image.Data[ii * 4 + 3]);
                                }
                                var info = new SpriteInfo()
                                {
                                    Name = Path.GetFileNameWithoutExtension(files[i]),
                                    PackId = packid,
                                    Width = image.Width,
                                    Height = image.Height,
                                };
                                spriteInfo.Add(info);
                                _packer.Add(packid, info.Width, info.Height, pixels);
                                packid++;
                            }
                        }
                    }
                }
            }

            _texturePages = _packer.Pack();

            {
                for (var i = 0; i < spriteInfo.Count; i++)
                {
                    if (spriteInfo[i].AseSprite != null)
                    {
                        var sprite = new Sprite();
                        if (spriteInfo[i].AseSprite.Slices.Count > 0 && spriteInfo[i].AseSprite.Slices[0].Pivot.HasValue)
                        {
                            var pivot = spriteInfo[i].AseSprite.Slices[0].Pivot.Value;
                            sprite.SetPivot(pivot);
                        }
                        if (spriteInfo[i].AseSprite.Tags.Count > 0)
                        {
                            for (var t = 0; t < spriteInfo[i].AseSprite.Tags.Count; t++)
                            {
                                var animation = new Sprite.Animation()
                                {
                                    Name = spriteInfo[i].AseSprite.Tags[t].Name
                                };
                                for (int f = spriteInfo[i].AseSprite.Tags[t].From; f <= spriteInfo[i].AseSprite.Tags[t].To; f++)
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
                                                Duration = spriteInfo[i].AseSprite.Frames[f].Duration / 1000.0f
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
                            for (int f = 0; f < spriteInfo[i].AseSprite.Frames.Count; f++)
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
                                            Duration = spriteInfo[i].AseSprite.Frames[f].Duration / 1000.0f
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
                    else
                    {
                        var sprite = new Sprite();
                        for (var p = 0; p < _texturePages.Length; p++)
                        {
                            Rectangle rect;
                            if (_texturePages[p].SubTextures.TryGetValue(spriteInfo[i].PackId, out rect))
                            {
                                var frame = new Sprite.Frame()
                                {
                                    Texture = _texturePages[p].Texture,
                                    Rect = rect,
                                    Duration = 0.0f
                                };
                                var animation = new Sprite.Animation()
                                {
                                    Name = "animation"
                                };
                                animation.Frames.Add(frame);
                                sprite.Animations.Add(animation);
                                _sprites[spriteInfo[i].Name] = sprite;
                                break;
                            }
                        }
                    }
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
