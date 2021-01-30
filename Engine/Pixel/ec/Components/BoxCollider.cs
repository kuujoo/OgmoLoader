﻿using Microsoft.Xna.Framework;

namespace kuujoo.Pixel
{
    public class BoxCollider : Collider
    {
        public override Rectangle Bounds => new Rectangle((Entity.Transform.Position + _position), _size.ToPoint());
        Point _position;
        Vector2 _size;
        public BoxCollider(Rectangle rect)
        {
            _position = rect.Location;
            _size = rect.Size.ToVector2();
        }
        public BoxCollider(int x, int y, int width, int height)
        {
            _position = new Point(x, y);
            _size = new Vector2(width, height);
        }
        public override bool Collides(Collider other, Point offset)
        {
            if (other is BoxCollider)
            {
                var otherbounds = (other as BoxCollider).Bounds;
                otherbounds.Location += offset;
                return CollisionChecks.RectAndRect(otherbounds, Bounds);
            }
            else if (other is GridCollider)
            {
                return CollisionChecks.RectAndGrid(Bounds, other as IGrid);
            }
            return false;
        }
        public void Resize(int x, int y, int width, int height)
        {
            _position = new Point(x, y);
            _size = new Vector2(width, height);
            Updated?.Invoke(this);
        }
        public void Resize(Rectangle rect)
        {
            _position = rect.Location;
            _size = rect.Size.ToVector2();
            Updated?.Invoke(this);
        }
        public override bool IsVisibleFromCamera(Camera camera)
        {
            return camera.Bounds.Intersects(Bounds);
        }
        public override void DebugRender(Graphics graphics)
        {
            graphics.DrawHollowRect(Bounds, Color.Red);
        }
    }
}
