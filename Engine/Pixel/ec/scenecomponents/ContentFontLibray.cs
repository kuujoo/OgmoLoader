using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace kuujoo.Pixel
{
    public class ContentFontLibray : SceneComponent, IFontLibrary
    {
        ContentManager _contentManager;
        Dictionary<string, BMFont> _fonts = new Dictionary<string, BMFont>();
        string _fontSheetContentPath;
        public ContentFontLibray(string[] fontXmlDirectory, string fontSheetContentPath)
        {
            _contentManager = new ContentManager(Engine.Instance.Content.ServiceProvider, Engine.Instance.Content.RootDirectory);
            _fontSheetContentPath = fontSheetContentPath;
            for (var i = 0; i < fontXmlDirectory.Length; i++)
            {
                var f = fontXmlDirectory[i];
                var files = Directory.GetFiles(f, "*.fnt", SearchOption.AllDirectories);
                for (var fi = 0; fi < files.Length; fi++)
                {
                    var file = files[fi];
                    LoadFont(file);
                }
            }
        }
        void LoadFont(string file)
        {
            var textureFile = FontData.GetTextureNameForFont(file);
            var contentTexture = Path.GetFileNameWithoutExtension(textureFile);
            var texture = _contentManager.Load<Texture2D>(Path.Combine(_fontSheetContentPath, contentTexture));
            using (var stream = TitleContainer.OpenStream(file))
            {
                var fontDesc = FontData.Load(stream);
                var font = new BMFont(texture, fontDesc);
                _fonts[Path.GetFileNameWithoutExtension(file)] = font;
            }
        }
        public override void CleanUp()
        {
            base.CleanUp();
            _contentManager.Unload();
        }

        public BMFont GetFont(string name)
        {
            return _fonts[name];
        }
    }
}
