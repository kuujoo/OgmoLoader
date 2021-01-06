using Microsoft.Xna.Framework;
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

    public abstract class ColliderComponent : Component
    {
        public Action<ColliderComponent> Updated;
        public Rectangle PhysicsBounds; // for spatialhash
        public abstract Rectangle Bounds { get; }
        public int Mask { get; set; }
        public abstract bool Collides(ColliderComponent collider);
        public ColliderComponent()
        {
            Mask = 0;
        }
        public override void TransformChanged(Transform transform)
        {
            Updated?.Invoke(this);
        }
        public ColliderComponent Check(int mask, Point point)
        {
            var bounds = Bounds;
            bounds.Location = point;
            var colliders = Entity.Scene.Tracker.Check(ref bounds, mask);
            foreach(var c in colliders)
            {
                if (c == this || !c.Enabled || (c.Mask & mask) == 0) continue;

                if( Collides(c) )
                {
                    return c;
                }
            }
            return null;
        }
    }
    public class BoxColliderComponent : ColliderComponent
    {
        public override Rectangle Bounds => new Rectangle( (Entity.Transform.Position + _position), _size.ToPoint());
        Point _position;
        Vector2 _size;
        public BoxColliderComponent(int x, int y, int width, int height)
        {
            _position = new Point(x, y);
            _size = new Vector2(width, height);
        }
        public override bool Collides(ColliderComponent other)
        {
            if (other is BoxColliderComponent)
            {
                var mybounds = Bounds;
                var otherbounds = (other as BoxColliderComponent).Bounds;
                return mybounds.Intersects(otherbounds);
            }
            return false;
        }
    }
}
