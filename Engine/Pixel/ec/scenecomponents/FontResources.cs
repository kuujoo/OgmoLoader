using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace kuujoo.Pixel
{
    public class FontResources : SceneComponent
    {
        public TexturePage[] TexturePages { get; private set; }
        Dictionary<string, BMFont> _fonts = new Dictionary<string, BMFont>();
        static string[] _fontExtensions = { "*.fnt" };
        string _fontdirectory;
        public FontResources(int texturepagewidth, int texturepageheight, string fontDirectory = "")
        {
            _fontdirectory = fontDirectory;
            BuildFonts();
        }
        void BuildFonts()
        {
            List<TextureInfo> textureInfo;
            TexturePagesBuilder builder = new TexturePagesBuilder();
            {
                if (_fontdirectory != "")
                {
                    for (var w = 0; w < _fontExtensions.Length; w++)
                    {
                        var files = Directory.GetFiles(_fontdirectory, _fontExtensions[w]);
                        if (files.Length > 0)
                        {
                            for (var i = 0; i < files.Length; i++)
                            {
                                var textureFile = FontData.GetTextureNameForFont(files[i]);
                                builder.Add(textureFile);
                            }

                            TexturePages = builder.Build(out textureInfo);

                            for (var i = 0; i < files.Length; i++)
                            {
                                var textureName = Path.GetFileNameWithoutExtension( FontData.GetTextureNameForFont(files[i]) );
                                for(var ti = 0; ti < textureInfo.Count; ti++)
                                {
                                    if(textureInfo[ti].Name == textureName)
                                    {

                                        for (var p = 0; p < TexturePages.Length; p++)
                                        {
                                            Rectangle rect;
                                            if (TexturePages[i].SubTextures.TryGetValue(textureInfo[ti].PackId, out rect))
                                            {
                                                using (var stream = TitleContainer.OpenStream(files[i]))
                                                {
                                                    var fontDesc = FontData.Load(stream);
                                                    for (var f = 0; f < fontDesc.Chars.Count; f++)
                                                    {
                                                        var data = fontDesc.Chars[f];
                                                        data.X += rect.X;
                                                        data.Y += rect.Y;
                                                    }
                                                    var font = new BMFont(TexturePages[i].Texture, fontDesc);
                                                    _fonts[Path.GetFileNameWithoutExtension(files[i])] = font;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public BMFont GetFont(string font)
        {
            return _fonts[font];
        }
        public override void CleanUp()
        {
            _fonts.Clear();
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
