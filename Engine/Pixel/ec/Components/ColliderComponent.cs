using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public abstract class ColliderComponent
    {
        public Entity Entity { get; set; }
        public Vector2 Position;
        public float AbsoluteX
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.Position.X + Position.X;
                }
                return Position.X;
            }
        }
        public float AbsoluteY
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.Position.Y + Position.Y;
                }
                return Position.Y;
            }
        }
        public Rectangle AbsoluteBounds => new Rectangle((int)AbsoluteX, (int)AbsoluteY, (int)Width, (int)Height);
        public abstract float Width { get; set; }
        public abstract float Height { get; set; }
        public abstract bool Collides(ColliderComponent collider);
    }

    public class BoxCollider : ColliderComponent
    {
        public override float Width { get; set; }
        public override float Height { get; set; }
        public BoxCollider(float x, float y, float width, float height)
        {
            Width = width;
            Height = height;
            Position.X = x;
            Position.Y = y;
        }
        public bool Intersects(BoxCollider other)
        {
            var me = AbsoluteBounds;
            var otherb = other.AbsoluteBounds;
            if (me.Left < otherb.Right && me.Right > otherb.Left && me.Bottom > otherb.Top)
            {
                return me.Top < otherb.Bottom;
            }
            else
            {
                return false;
            }
        }
        public override bool Collides(ColliderComponent collider)
        {
            if(collider is BoxCollider)
            {
                return Intersects(collider as BoxCollider);
            } 
            else
            {
                return false;
            }
        }
    }

}
