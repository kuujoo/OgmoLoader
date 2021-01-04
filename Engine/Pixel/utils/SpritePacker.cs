using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace kuujoo.Pixel
{
    public class SpritePacker
    {
        class PixelData : IComparable<PixelData>
        {
            public string Name;
            public int Frame;
            public int Width;
            public int Height;
            public Color[] Pixels;
            public PixelData(int width, int height, int frame)
            {
                Width = width;
                Height = height;
                Pixels = new Color[Width * Height];
                Frame = frame;
            }

            public int CompareTo( PixelData other)
            {
                int area = Width * Height;
                int other_Area = other.Width * other.Height;
                return other_Area.CompareTo(area);
            }

            public void Add(int x, int y, PixelData data)
            {
                for (int j = 0; j < data.Height; j++)
                {
                    var src_idx = j * data.Width;
                    var dst_idx = (y + j) * Width + x;
                    Array.Copy(data.Pixels, src_idx, Pixels, dst_idx, data.Width);
                }
            }
            public void Clear()
            {
                Array.Fill<Color>(Pixels, Color.Transparent);
            }
        }
        struct PackedData
        {
            public Texture2D Texture;
            public List<Rectangle> Bounds;
        }

        List<PixelData> _packBuffer = new List<PixelData>();
        List<Texture2D> _texturePages = new List<Texture2D>();
        int _pageWidth;
        int _pageHeight;
        public SpritePacker(int texturepagewidth, int texturepageheight)
        {
            _pageWidth = texturepagewidth;
            _pageHeight = texturepageheight;
        }
        public void Add(string name, int width, int height, int frame, Color[] pixels)
        {
            var pixeldata = new PixelData(width, height, frame)
            {
                Name = name,
                Pixels = pixels
            };

            _packBuffer.Add(pixeldata);
        }
        Texture2D CreateTexturePage()
        {
            var texture = new Texture2D(Engine.Instance.GraphicsDevice, _pageWidth, _pageHeight);
            _texturePages.Add(texture);
            return texture;
        }
        Point CalculateRequiredSize(List<PixelData> data, int index)
        {
            int required__width = data[index].Width;
            int required_height = data[index].Height;
            if (index < data.Count - 1 && data[index].Frame == 0)
            {
                for (int j = index + 1; j < data.Count; j++)
                {
                    if (data[index].Frame == data[j].Frame - 1 && data[index].Name == data[index].Name)
                    {
                        required__width += data[j].Width;
                        required_height += data[j].Height;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return new Point(required__width, required_height);
        }
        public Dictionary<string, Sprite> Pack(out Texture2D[] texturepages)
        {
            _packBuffer.Sort();
            Dictionary<string, PackedData> packed_data = new Dictionary<string, PackedData>();
            var texture = CreateTexturePage();
            PixelData packet = new PixelData(_pageWidth, _pageHeight, 0);
            int addx = 0;
            int addy = 0;
            int row_height = 0;
            for (var i = 0; i < _packBuffer.Count; i++)
            {
                var s = CalculateRequiredSize(_packBuffer, i);
                int required__width = s.X;
                int required_height = s.Y;
                if (addx + required__width > packet.Width)
                {
                    addx = 0;
                    addy += row_height;
                }
                if (addy + required_height > packet.Height)
                {
                    texture.SetData<Color>(packet.Pixels);
                    texture = CreateTexturePage();
                    row_height = 0;
                    addy = 0;
                    addx = 0;
                    packet.Clear();
                }

                packet.Add(addx, addy, _packBuffer[i]);           
                if(!packed_data.ContainsKey(_packBuffer[i].Name))
                {
                    packed_data[_packBuffer[i].Name] = new PackedData()
                    {
                        Texture = texture,
                        Bounds = new List<Rectangle>()
                    };
                }
                var bounds = new Rectangle(addx, addy, _packBuffer[i].Width, _packBuffer[i].Height);
                packed_data[_packBuffer[i].Name].Bounds.Add(bounds);
                addx += _packBuffer[i].Width;
                row_height = Math.Max(row_height, _packBuffer[i].Height);  
            }
            texture.SetData<Color>(packet.Pixels);

            Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
            foreach (var p in packed_data)
            {
                var sprite = new Sprite(p.Value.Texture, p.Value.Bounds.ToArray());
                sprites[p.Key] = sprite;
            }
            texturepages = _texturePages.ToArray();
            _texturePages.Clear();
            return sprites;
        }     
    }
}
