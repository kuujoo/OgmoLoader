﻿using Microsoft.Xna.Framework;
using System;

namespace kuujoo.Pixel
{
    public class GridCollider : Collider, IGrid, IRenderable
    {
        public int Layer { get; set; }
        public int CellWidth { get; private set; }
        public int CellHeight { get; private set; }
        public override Rectangle Bounds => new Rectangle((Entity.Transform.Position), new Point(Width * CellWidth, Height * CellHeight));
        public int Width { get; private set; }
        public int Height { get; private set; }
        byte[] _grid;
        public GridCollider(int width, int height, int cellw, int cellh)
        {
            Width = width;
            Height = height;
            CellWidth = cellw;
            CellHeight = cellh;
            _grid = new byte[Width * Height];
        }

        public byte GetValue(int x, int y)
        {
            return GetValueByIndex(y * Width + x);
        }

        public byte GetValueByIndex(int idx)
        {
            if (idx >= _grid.Length || idx < 0) return 0;
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
        public override bool Collides(Collider other, Point offset)
        {
            if (other is BoxCollider)
            {
                var r = other.Bounds;
                r.Location -= Entity.Transform.Position;
                r.Location += offset;
                return CollisionChecks.RectAndGrid(r, this, CellWidth, CellHeight);            
            }
            return false;
        }
        public bool IsVisibleFromCamera(Camera camera)
        {
            return true;
        }
        public void Render(Graphics graphics)
        {
        }
        public void DebugRender(Graphics graphics)
        {
            var bounds = graphics.Camera.Bounds;
            bounds.Location -= Entity.Transform.Position;
            int left = Math.Clamp((int)Math.Floor((float)bounds.Left / CellWidth), 0, Width);
            int right = Math.Clamp((int)Math.Floor((float)bounds.Right / CellWidth), 0, Width);
            int top = Math.Clamp((int)Math.Floor((float)bounds.Top / CellHeight), 0, Height);
            int bottom = Math.Clamp((int)Math.Floor((float)bounds.Bottom / CellHeight), 0, Height);
            for (var j = top; j <= bottom; j++)
            {
                for (var i = left; i <= right; i++)
                {
                    if(GetValue(i, j) != 0)
                    {
                        graphics.DrawHollowRect(new Rectangle(Entity.Transform.Position.X + i * CellWidth, Entity.Transform.Position.Y + j * CellHeight, CellWidth, CellHeight), Color.Red);
                    }
                }
            }
        }
    }
}