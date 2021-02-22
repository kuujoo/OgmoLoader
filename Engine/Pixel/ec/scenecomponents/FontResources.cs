using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using kuujoo.Pixel.Packer;

namespace kuujoo.Pixel
{
    public class FontResources : SceneComponent
    {
        public TexturePage[] TexturePages { get; private set; }
        Dictionary<string, BMFont> _fonts = new Dictionary<string, BMFont>();
        static string[] _fontExtensions = { "*.fnt" };
        string[] _fontdirectory;
        int _texturepagewidth;
        int _texturepageheight;
        public FontResources(int texturepagewidth, int texturepageheight, string[] fontDirectories)
        {
            _texturepagewidth = texturepagewidth;
            _texturepageheight = texturepageheight;
            _fontdirectory = fontDirectories;
            BuildFonts();
        }
        void BuildFonts()
        {
            SpritePacker packer = new SpritePacker(_texturepagewidth, _texturepageheight);
            Dictionary<string, string> fontToSubimage = new Dictionary<string, string>();
            for (var d = 0; d < _fontdirectory.Length; d++)
            {
                for (var i = 0; i < _fontExtensions.Length; i++)
                {
                    var files = Directory.GetFiles(_fontdirectory[d], _fontExtensions[i], SearchOption.AllDirectories);

                    for (var f = 0; f < files.Length; f++)
                    {
                        var textureFile = FontData.GetTextureNameForFont(files[i]);
                        packer.Add(textureFile);
                        fontToSubimage[files[i]] = Path.GetFileNameWithoutExtension(textureFile);
                    }
                }
            }
            var atlasses = packer.Pack();
            TexturePages = new TexturePage[atlasses.Count];
            for (var a = 0; a < atlasses.Count; a++)
            {
                TexturePages[a] = TexturePage.FromAtlas(Engine.Instance.Graphics.Device, atlasses[a]);
            }

            foreach (var ft in fontToSubimage)
            {
                var fntFile = ft.Key;
                var subtextureName = ft.Value;
                for (var p = 0; p < TexturePages.Length; p++)
                {
                    var sub = TexturePages[p].SubTextures;
                    for (var s = 0; s < sub.Count; s++)
                    {
                        var info = sub[s];
                        if (info.Name == subtextureName)
                        {
                            using (var stream = TitleContainer.OpenStream(fntFile))
                            {
                                var fontDesc = FontData.Load(stream);
                                for (var f = 0; f < fontDesc.Chars.Count; f++)
                                {
                                    var data = fontDesc.Chars[f];
                                    data.X += info.X;
                                    data.Y += info.Y;
                                }
                                var font = new BMFont(TexturePages[p].Texture, fontDesc);
                                _fonts[Path.GetFileNameWithoutExtension(fntFile)] = font;
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
