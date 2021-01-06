﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public static class CollisionMask
    {
        public static int Solid = 1 << 0;
        public static int JumpThroughSolid = 1 << 1;
        public static int Danger = 1 << 2;
        public static int Actor = 1 << 3;
        public static int Player = 1 << 4;
        public static int Trigger = 1 << 5;
        public static int Enemy = 1 << 6;
    }

    public abstract class Collider : Component
    {
        public Action<Collider> Updated;
        public Rectangle PhysicsBounds; // for spatialhash
        public abstract Rectangle Bounds { get; }
        public int Mask { get; set; }
        public abstract bool Collides(Collider collider, Point offset);
        public Collider()
        {
            Mask = 0;
        }
        public override void Initialize()
        {
            base.Initialize();
            Entity.Scene.Physics.RegisterCollider(this);
        }
        public override void CleanUp()
        {
            base.CleanUp();
            Entity.Scene.Physics.UnregisterCollider(this);
        }
        public override void TransformChanged(Transform transform)
        {
            Updated?.Invoke(this);
        }
        public Collider Check(int mask, Point offset)
        {
            var bounds = Bounds;
            bounds.Location += offset;
            var colliders = Entity.Scene.Physics.Check(ref bounds, mask);
            for(var i = 0; i < colliders.Count; i++)
            {
                var c = colliders[i];
                if (c == this || !c.Enabled || (c.Mask & mask) == 0) continue;

                if( c.Collides(this, offset) )
                {
                    return c;
                }
            }
            return null;
        }
    }
    public class BoxCollider : Collider
    {
        public override Rectangle Bounds => new Rectangle((Entity.Transform.Position + _position), _size.ToPoint());
        Point _position;
        Vector2 _size;
        public BoxCollider(int x, int y, int width, int height)
        {
            _position = new Point(x, y);
            _size = new Vector2(width, height);
        }
        public override bool Collides(Collider other, Point offset)
        {
            if (other is BoxCollider)
            {
                var mybounds = Bounds;
                var otherbounds = (other as BoxCollider).Bounds;
                otherbounds.Location += offset;
                return mybounds.Intersects(otherbounds);
            }
            else if (other is GridCollider)
            {
            }
            return false;
        }
        public override void Render(Graphics graphics)
        {
            graphics.DrawHollowRect(Bounds, Color.Red);
        }
    }
    public class GridCollider : Collider, IGrid
    {
        public override Rectangle Bounds => new Rectangle((Entity.Transform.Position), new Point(Width * _cellW, Height * _cellH));
        public int Width { get; private set; }
        public int Height { get; private set; }
        byte[] _grid;
        int _cellW;
        int _cellH;
        byte Free = 0;
        public GridCollider(int width, int height, int cellw, int cellh)
        {
            Width = width;
            Height = height;
            _cellW = cellw;
            _cellH = cellh;
            _grid = new byte[Width * Height];
        }

        public byte GetValue(int x, int y)
        {
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
        public override bool Collides(Collider other, Point offset)
        {
            if (other is BoxCollider)
            {
                var r = other.Bounds;
                r.Location += offset;

                var left = (int)Math.Clamp((float)r.Left / _cellW, 0, Width);
                var right = (int)Math.Clamp((float)(r.Right) / _cellW, 0, Width);
                var up = (int)Math.Clamp((float)r.Top / _cellH, 0, Height);
                var down = (int)Math.Clamp((float)(r.Bottom) / _cellH, 0, Height);

                for(var i = left; i <= right; i++)
                {
                    for(var j = up; j <= down; j++)
                    {
                        var v = GetValue(i, j);
                        if(v != Free)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public override void Render(Graphics graphics)
        {
            base.Render(graphics);
            var bounds = graphics.Camera.Bounds;
            int left = (int)Math.Floor((float)bounds.Left / _cellW);
            int right = (int)Math.Ceiling((float)bounds.Right / _cellW);
            int top = bounds.Top / _cellH;
            int bottom = bounds.Bottom / _cellH;
            for (var j = top; j < bottom; j++)
            {
                for (var i = left; i < right; i++)
                {
                    if(GetValue(i, j) != 0)
                    {
                        graphics.DrawHollowRect(new Rectangle(Entity.Transform.Position.X + i * _cellW, Entity.Transform.Position.Y + j * _cellH, _cellW, _cellH), Color.Red);
                    }
                }
            }
        }
    }
}
