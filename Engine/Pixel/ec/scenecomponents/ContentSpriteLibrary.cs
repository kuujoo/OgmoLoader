using Microsoft.Xna.Framework.Content;
using System.IO;
using kuujoo.Pixel.Packer;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public class ContentSpriteLibrary : SceneComponent, ISpriteLibrary
    {
        ContentManager _contentManager;
        Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        string _spritesheetsContentPath;
        public ContentSpriteLibrary(string[] spriteSheetJsonDirectory, string spritesheetsContentPath)
        {
            _contentManager = new ContentManager(Engine.Instance.Content.ServiceProvider, Engine.Instance.Content.RootDirectory);
            _spritesheetsContentPath = spritesheetsContentPath;
            for (var i = 0; i < spriteSheetJsonDirectory.Length; i++)
            {
                var f = spriteSheetJsonDirectory[i];
                var files = Directory.GetFiles(f, "*.json", SearchOption.AllDirectories);
                for(var fi = 0; fi < files.Length; fi++)
                {
                    var file = files[fi];
                    LoadSpriteSheet(file);
                }
            }
        }
        public override void CleanUp()
        {
            base.CleanUp();
            _contentManager.Unload();
        }
        void LoadSpriteSheet(string sheetJson)
        {
            var data = File.ReadAllText(sheetJson);
            List<Atlas.SubImage> subtextures = System.Text.Json.JsonSerializer.Deserialize<List<Atlas.SubImage>>(data);
            var filename = Path.GetFileNameWithoutExtension(sheetJson);
            var texture = _contentManager.Load<Texture2D>(Path.Combine(_spritesheetsContentPath, filename));

            for (var s = 0; s < subtextures.Count; s++)
            {
                var info = subtextures[s];
                Sprite sprite;
                if (!_sprites.TryGetValue(info.Name, out sprite))
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
                if (animation == null)
                {
                    animation = new Sprite.Animation();
                    animation.Name = info.Tag;
                    sprite.Animations.Add(animation);
                }
                var frame = new Sprite.Frame()
                {
                    Id = info.Frame,
                    Texture = texture,
                    Rect = new Rectangle(info.X, info.Y, info.Width, info.Height),
                    Duration = (float)info.Duration / 1000.0f
                };
                animation.AddFrame(frame);
            }
        }
        public Sprite GetSprite(string name)
        {
            return _sprites[name];
        }
    }
}
