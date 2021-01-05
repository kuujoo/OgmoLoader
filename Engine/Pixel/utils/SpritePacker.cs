using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace kuujoo.Pixel
{
    public class SpritePacker
    {
        public class PixelRect : BinaryPackerRect
        {
            public override int Width { get; set; }
            public override int Height { get; set; }
            public string Name { get; set; }
            public string Tag { get; set; }
            public Color[] Pixels;
            public int Frames { get; set; }
            public PixelRect(string name, string tag, int frames, int width, int height)
            {
                Name = name;
                Tag = tag;
                Width = width;
                Height = height;
                Frames = frames;
                Pixels = new Color[width * height];
            }
        }
        List<PixelRect> _packBuffer = new List<PixelRect>();
        List<Texture2D> _texturePages = new List<Texture2D>();
        int _pageWidth;
        int _pageHeight;
        public SpritePacker(int texturepagewidth, int texturepageheight)
        {
            _pageWidth = texturepagewidth;
            _pageHeight = texturepageheight;
        }
        public void Add(PixelRect rect)
        {
            _packBuffer.Add(rect);
        }
        Texture2D CreateTexturePage()
        {
            var texture = new Texture2D(Engine.Instance.GraphicsDevice, _pageWidth, _pageHeight);
            _texturePages.Add(texture);
            return texture;
        }
        public Dictionary<string, List<Sprite>> Pack(out Texture2D[] texturepages)
        { 
            Dictionary<string, List<Sprite>> sprites = new Dictionary<string, List<Sprite>>();
            while (_packBuffer.Count > 0)
            {      
                int idx = BinaryPacker.Pack(_pageWidth, _pageHeight, ref _packBuffer);
                var texture = CreateTexturePage();
                for (var i = 0; i < idx; i++)
                {
                    var pack = _packBuffer[i];
                    var rect = pack.Bounds;
                    texture.SetData(0, rect, pack.Pixels, 0, pack.Pixels.Length);
                    var true_w = pack.Width / pack.Frames;
                    Rectangle[] rects = new Rectangle[pack.Frames];
                    for (var kk = 0; kk < pack.Frames; kk++)
                    {
                        rects[kk] = new Rectangle(rect.X + kk * true_w, rect.Y, true_w, rect.Height);
                    }

                    if(!sprites.ContainsKey(pack.Name))
                    {
                        sprites[pack.Name] = new List<Sprite>();                     
                    }
                    sprites[pack.Name].Add(new Sprite(texture, rects, pack.Tag));

                }
                _packBuffer.RemoveRange(0, idx);
            }
            texturepages = _texturePages.ToArray();
            _texturePages.Clear();
            return sprites;
        }     
    }
}
