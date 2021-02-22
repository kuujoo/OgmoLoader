using System.Collections.Generic;
using System.IO;
using kuujoo.Pixel.Packer;
using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public interface ISpriteLibrary
    {
        public Sprite GetSprite(string name);
    }

    public class RuntimeSpriteLibrary : SceneComponent, ISpriteLibrary
    {
        public TexturePage[] TexturePages { get; private set; }
        Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        Dictionary<string, BMFont> _fonts = new Dictionary<string, BMFont>();
        string[] _spritedirectory;
        static string[] _imageExtensions = { "*.ase", "*.png", "*.jpg", "*.jpeg", "*.bmp" };
        int _texturepagewidth;
        int _texturepageheight;
        public RuntimeSpriteLibrary(int texturepagewidth, int texturepageheight, string[] spritedirectory)
        {
            _spritedirectory = spritedirectory;
            _texturepagewidth = texturepagewidth;
            _texturepageheight = texturepageheight;
            BuildSprites();
        }
        void BuildSprites()
        {
            SpritePacker packer = new SpritePacker(_texturepagewidth, _texturepageheight);
            for (var d = 0; d < _spritedirectory.Length; d++)
            {
                for (var i = 0; i < _imageExtensions.Length; i++)
                {
                    var files = Directory.GetFiles(_spritedirectory[d], _imageExtensions[i], SearchOption.AllDirectories);
                    for (var f = 0; f < files.Length; f++)
                    {
                        packer.Add(files[f]);
                    }
                }
            }
            var atlasses = packer.Pack();
            TexturePages = new TexturePage[atlasses.Count];
            for (var a = 0; a < atlasses.Count; a++)
            {
                TexturePages[a] = TexturePage.FromAtlas(Engine.Instance.Graphics.Device, atlasses[a]);
            }
            
            for(var i = 0; i< TexturePages.Length; i++)
            {
                var subtextures = TexturePages[i].SubTextures;
                for(var s = 0; s < subtextures.Count; s++)
                {
                    var info = subtextures[s];
                    Sprite sprite;
                    if(!_sprites.TryGetValue(info.Name, out sprite))
                    {
                        sprite = new Sprite();
                        _sprites[info.Name] = sprite;
                    }
                    if (info.Slices != null && info.Slices.Length != 0 && info.Frame == 0)
                    {
                        var slice = info.Slices[0];
                        sprite.SetPivot(new Vector2(slice.PivotX, slice.PivotY));
                    }
                    Sprite.Animation animation = null;
                    if (info.Tag != "")
                    {
                        animation = sprite.GetAnimation(info.Tag);
                    }
                    if(animation == null)
                    {
                        animation = new Sprite.Animation();
                        animation.Name = info.Tag;
                        sprite.Animations.Add(animation);
                    }
                    var frame = new Sprite.Frame()
                    {
                        Id = info.Frame,
                        Texture = TexturePages[i].Texture,
                        Rect = new Rectangle(info.X, info.Y, info.Width, info.Height),
                        Duration = (float)info.Duration / 1000.0f
                    };
                    animation.AddFrame(frame);
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
