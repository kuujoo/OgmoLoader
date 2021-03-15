using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace kuujoo.Pixel
{

    public class Grid<T>
    {
        public T[] Data => _grid;
        T[] _grid;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            _grid = new T[Width * Height];
        }
        public T GetValue(int x, int y)
        {
            return GetValueByIndex(y * Width + x);
        }
        public T GetValueByIndex(int idx)
        {
            return _grid[idx];
        }
        public void SetValue(int x, int y, T value)
        {
            SetValueByIndex(y * Width + x, value);
        }
        public void SetValueByIndex(int index, T value)
        {
            _grid[index] = value;
        }
        public void SetFrom(Grid<T> grid, int x, int y)
        {
            for (var i = 0; i < grid.Width; i++)
            {
                for (var j = 0; j < grid.Height; j++)
                {
                    SetValue(i + x, j + y, grid.GetValue(i, j));
                }
            }
        }
        public void SetValues(T value)
        {
            SetValueByRect(new Rectangle(0, 0, Width, Height), value);
        }
        public void SetValueByRect(Rectangle rect, T value)
        {
            for (var i = rect.X; i < rect.X + rect.Width; i++)
            {
                for (var j = rect.Y; j < rect.Y + rect.Height; j++)
                {
                    SetValue(i, j, value);
                }
            }
        }
        public void SetValueByRange(Point p0, Point size, T value)
        {
            for (var i = p0.X; i < p0.X + size.X; i++)
            {
                for (var j = p0.Y; j < p0.Y + size.Y; j++)
                {
                    SetValue(i, j, value);
                }
            }
        }
        public void SetReverseFrom(Grid<T> grid, int x, int y)
        {
            for (var i = 0; i < grid.Width; i++)
            {
                for (var j = 0; j < grid.Height; j++)
                {
                    SetValue(i + x, j + y, grid.GetValue((grid.Width - 1) - i, j));
                }
            }
        }
        public void SetBorders(T value)
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

    [Serializable]
    public class ByteGrid : Grid<byte>
    {
        public ByteGrid(int width, int height) : base(width, height)
        {

        }
    }
    public class FloatGrid : Grid<float>
    {
        public FloatGrid(int width, int height) : base(width, height)
        {

        }
    }

    public class IntGrid : Grid<int>
    {
        public IntGrid(int width, int height) : base(width, height)
        {

        }
    }

    public class ColorGrid : Grid<Color>
    {
        public ColorGrid(int width, int height) : base(width, height)
        {

        }
    }
}
