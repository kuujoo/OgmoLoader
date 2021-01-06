using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace kuujoo.Pixel
{
    public class TexturePage : IDisposable
    {
        public Texture2D Texture { get; set; }
        public Dictionary<int, Rectangle> SubTextures {get;set;}
        public TexturePage()
        {
            SubTextures = new Dictionary<int, Rectangle>();
        }

        void Dispose(bool dispose)
        {
            if(dispose && Texture != null)
            {
                Texture.Dispose();
                Texture = null;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    public class SpritePacker
    {
        public class SpritePixelData : BinaryPackerRect
        {
            public int Id { get; set; }
            public override int Width { get; set; }
            public override int Height { get; set; }
            public Color[] Pixels;
            public SpritePixelData(int id, int width, int height, Color[] pixels)
            {
                Id = id;
                Width = width;
                Height = height;
                Pixels = pixels;
            }
        }
        List<SpritePixelData> _packBuffer = new List<SpritePixelData>();
        List<TexturePage> _texturePages = new List<TexturePage>();
        int _pageWidth;
        int _pageHeight;
        public SpritePacker(int texturepagewidth, int texturepageheight)
        {
            _pageWidth = texturepagewidth;
            _pageHeight = texturepageheight;
        }
        public void Add(int packid, int width, int height, Color[] pixels)
        {
            _packBuffer.Add(new SpritePixelData(packid, width, height, pixels));
        }
        TexturePage CreateTexturePage()
        {
            var texturepage = new TexturePage()
            {
                Texture = new Texture2D(Engine.Instance.GraphicsDevice, _pageWidth, _pageHeight)
            };
            _texturePages.Add(texturepage);
            return texturepage;
        }
        public TexturePage[] Pack()
        {
            while (_packBuffer.Count > 0)
            {
                int idx = BinaryPacker.Pack(_pageWidth, _pageHeight, ref _packBuffer);
                var texturepage = CreateTexturePage();
                for (var i = 0; i < idx; i++)
                {
                    var pack = _packBuffer[i];
                    var rect = pack.Bounds;
                    texturepage.Texture.SetData(0, rect, pack.Pixels, 0, pack.Pixels.Length);
                    texturepage.SubTextures[pack.Id] = rect;
                }
                _packBuffer.RemoveRange(0, idx);
            }
            var pages = _texturePages.ToArray();
            _texturePages.Clear();
            return pages;
        }
    }
}
