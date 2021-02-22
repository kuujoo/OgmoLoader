using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace kuujoo.Pixel
{


    public class SpriteResources : SceneComponent
    {
        public TexturePage[] TexturePages { get; private set; }
        Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        Dictionary<string, BMFont> _fonts = new Dictionary<string, BMFont>();
        string[] _spritedirectory;
        static string[] _imageExtensions = { "*.ase", "*.png", "*.jpg", "*.jpeg", "*.bmp" };
        public SpriteResources(int texturepagewidth, int texturepageheight, string[] spritedirectory)
        {
            _spritedirectory = spritedirectory;
          
            BuildSprites();
        }
        void BuildSprites()
        {
            TexturePagesBuilder builder = new TexturePagesBuilder();
            List<TextureInfo> spriteInfo;
            { 
                for (var w = 0; w < _imageExtensions.Length; w++)
                {
                    for (var f = 0; f < _spritedirectory.Length; f++)
                    {
                        var files = Directory.GetFiles(_spritedirectory[f], _imageExtensions[w]);
                        if (files.Length > 0)
                        {
                            for (var i = 0; i < files.Length; i++)
                            {
                                builder.Add(files[i]);
                            }
                        }
                    }
                }
            }

            TexturePages = builder.Build(out spriteInfo);

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
                                    for (var p = 0; p < TexturePages.Length; p++)
                                    {
                                        Rectangle rect;
                                        if (TexturePages[p].SubTextures.TryGetValue(spriteInfo[i].PackId + f, out rect))
                                        {
                                            var frame = new Sprite.Frame()
                                            {
                                                Texture = TexturePages[p].Texture,
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
                                for (var p = 0; p < TexturePages.Length; p++)
                                {
                                    Rectangle rect;
                                    if (TexturePages[p].SubTextures.TryGetValue(spriteInfo[i].PackId + f, out rect))
                                    {
                                        var frame = new Sprite.Frame()
                                        {
                                            Texture = TexturePages[p].Texture,
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
                        for (var p = 0; p < TexturePages.Length; p++)
                        {
                            Rectangle rect;
                            if (TexturePages[p].SubTextures.TryGetValue(spriteInfo[i].PackId, out rect))
                            {
                                var frame = new Sprite.Frame()
                                {
                                    Texture = TexturePages[p].Texture,
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
            if (TexturePages != null)
            {
                for (var i = 0; i < TexturePages.Length; i++)
                {
                    TexturePages[i].Dispose();
                }
            }
            TexturePages = null;
        }
    }
}
