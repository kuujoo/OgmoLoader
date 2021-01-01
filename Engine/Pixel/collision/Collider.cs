using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public abstract class Collider
    {
        public Entity Entity { get; set; }
        public Microsoft.Xna.Framework.Vector2 Position;
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
        public float AbsoluteRight => Entity.Position.X + Right;
        public float AbsoluteLeft => Entity.Position.X + Left;
        public float AbsoluteTop => Entity.Position.Y - Top;
        public float AbsoluteBottom => Entity.Position.Y + Bottom;
        public abstract float Left { get; set; }
        public abstract float Right { get; set; }
        public abstract float Top { get; set; }
        public abstract float Bottom { get; set; }
        public abstract float Width { get; set; }
        public abstract float Height { get; set; }
        public abstract bool Collides(Collider collider);
    }

    public class Hitbox : Collider
    {
        public override float Left
        {
            get
            {
                return Position.X;
            }
            set
            {
                Position.X = value;
            }
        }
        public override float Right
        {
            get
            {
                return Position.X + Width;
            }
            set
            {
                Position.X = value - Width;
            }
        }
        public override float Top
        { 
            get
            {
                return Position.Y;
            }
            set
            {
                Position.Y = value;
            }
        }
        public override float Bottom
        {
            get
            {
                return Position.Y + Height;
            }
            set
            {
                Position.Y = value - Height;
            }
        }
        public override float Width { get; set; }
        public override float Height { get; set; }
         public Hitbox(float x, float y, float width, float height)
        {
            Width = width;
            Height = height;
            Position.X = x;
            Position.Y = y;
        }
        public bool Intersects(Hitbox hitbox)
        {
            if (AbsoluteLeft < hitbox.AbsoluteRight && AbsoluteRight > hitbox.AbsoluteLeft && AbsoluteBottom > hitbox.AbsoluteTop)
            {
                return AbsoluteTop < hitbox.AbsoluteBottom;
            }
            else
            {
                return false;
            }
        }
        public override bool Collides(Collider collider)
        {
            if(collider is Hitbox)
            {
                return Intersects(collider as Hitbox);
            } 
            else
            {
                return false;
            }
        }
    }

}
