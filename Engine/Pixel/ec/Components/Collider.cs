using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public static class CollisionChecks
    {
        public static bool RectAndGrid(Rectangle rect, IGrid grid)
        {
            var r = rect;          
            var left = (int)Math.Clamp((float)r.Left / grid.CellWidth, 0, grid.Width);
            var right = (int)Math.Clamp( Math.Ceiling( (float)(r.Right) / grid.CellWidth), 0, grid.Width);
            var up = (int)Math.Clamp((float)r.Top / grid.CellHeight, 0, grid.Height);
            var down = (int)Math.Clamp(Math.Ceiling((float)(r.Bottom) / grid.CellHeight), 0, grid.Height);

            for (var i = left; i < right; i++)
            {
                for (var j = up; j < down; j++)
                {
                    var v = grid.GetValue(i, j);
                    if (v != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool RectAndRect(Rectangle rect0, Rectangle rect1)
        {
            return rect0.Intersects(rect1);
        }
    }
    public static class CollisionMask
    {
        public static int Solid = 1 << 0;
        public static int JumpThroughSolid = 1 << 1;
        public static int Danger = 1 << 2;
        public static int Actor = 1 << 3;
        public static int Player = 1 << 4;
        public static int Trigger = 1 << 5;
        public static int Enemy = 1 << 6;
        public static int Death = 1 << 7;
        public static int Interactable = 1 << 8;
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
}
