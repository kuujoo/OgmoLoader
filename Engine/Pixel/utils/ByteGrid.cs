using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public interface IGrid
    {
        public int Width { get; }
        public int Height { get; }
        byte GetValue(int x, int y);
        byte GetValueByIndex(int idx);
        void SetValue(int x, int y, byte value);
        void SetValueByIndex(int index, byte value);
        void SetFrom(IGrid grid, int x, int y);
    }

    [Serializable]
    public class ByteGrid : IGrid
    {
        byte[] _grid;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public ByteGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _grid = new byte[Width * Height];
        }
        public byte GetValue(int x, int y)
        {
            if (x < 0 || y < 0 || x > Width - 1 || y > Height - 1) return 0;
            return GetValueByIndex(y * Width + x);
        }
        public byte GetValueByIndex(int idx)
        {
            return _grid[idx];
        }
        public void SetValue(int x, int y, byte value)
        {
            SetValueByIndex(y * Width + x, value);
        }
        public void SetValueByIndex(int index, byte value)
        {
            _grid[index] = value;
        }
        public void SetFrom(IGrid grid, int x, int y)
        {
            for(var i = 0; i < grid.Width; i++)
            {
                for(var j = 0; j < grid.Height; j++)
                {
                    SetValue(i + x, j + y, grid.GetValue(i, j));
                }
            }
        }
        public void SetValueByRange(Point p0, Point size, byte value)
        {
            for(var i = p0.X; i < p0.X + size.X; i++)
            {
                for (var j = p0.Y; j < p0.Y + size.Y; j++)
                {
                    SetValue(i, j, value);
                }
            }
        }
        public void SetReverseFrom(IGrid grid, int x, int y)
        {
            for (var i = 0; i < grid.Width; i++)
            {
                for (var j = 0; j < grid.Height; j++)
                {
                    SetValue(i + x, j + y, grid.GetValue( (grid.Width-1) - i, j));
                }
            }
        }
        public void SetBorders(byte value)
        {
            for (var i = 0; i < Width; i++)
            {
                SetValue(i, 0, value);
                SetValue(i, Height - 1, value);
            }
            for (var i = 0; i < Height; i++)
            {
                SetValue(0, i, value);
                SetValue(Width - 1, i, value);
            }
        }
    }
}
