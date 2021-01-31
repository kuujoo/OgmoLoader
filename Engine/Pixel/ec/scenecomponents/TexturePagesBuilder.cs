using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using kuujoo.Pixel.StbImageSharp;

namespace kuujoo.Pixel
{
    public struct TextureInfo
    {
        public string Name;
        public int PackId;
        public int Width;
        public int Height;
        public AseSprite AseSprite;
    }

    public class TexturePagesBuilder
    {
        SpritePacker _packer;
        List<TextureInfo> _spriteInfo = new List<TextureInfo>();
        int _packId = 0;
        public TexturePagesBuilder()
        {
            _packer = new SpritePacker(4096, 4096);
        }

        public void Add(string file)
        {
            if (Path.GetExtension(file) == ".ase")
            {
                var ase = new AseSprite(file);
                var spriteInfo = new TextureInfo()
                {
                    AseSprite = ase,
                    Name = Path.GetFileNameWithoutExtension(file),
                    PackId = _packId,
                    Width = ase.Width,
                    Height = ase.Height

                };

                _spriteInfo.Add(spriteInfo);

                for (var j = 0; j < spriteInfo.AseSprite.FrameCount; j++)
                {
                    _packer.Add(_packId, ase.Width, ase.Height, spriteInfo.AseSprite.Frames[j].Pixels);
                    _packId++;
                }
            }
            else
            {
                using (var stream = File.OpenRead(file))
                {
                    ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                    Color[] pixels = new Color[image.Width * image.Height];
                    for (var ii = 0; ii < image.Width * image.Height; ii++)
                    {
                        pixels[ii] = new Color(image.Data[ii * 4 + 0], image.Data[ii * 4 + 1], image.Data[ii * 4 + 2], image.Data[ii * 4 + 3]);
                    }
                    var info = new TextureInfo()
                    {
                        Name = Path.GetFileNameWithoutExtension(file),
                        PackId = _packId,
                        Width = image.Width,
                        Height = image.Height,
                    };
                    _spriteInfo.Add(info);
                    _packer.Add(_packId, info.Width, info.Height, pixels);
                    _packId++;
                }
            }
        }
        public TexturePage[] Build(out List<TextureInfo> spriteInfos)
        {
            spriteInfos = _spriteInfo;
            return _packer.Pack();
        }
    }
}
