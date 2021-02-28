using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class BoxCollider : Collider, IRenderable
    {
        public int Layer => 0;
        public override Rectangle Bounds => new Rectangle((Entity.Transform.Position + _position), _size.ToPoint());
        Point _position;
        Vector2 _size;
        SubpixelFloat _castX = new SubpixelFloat();
        SubpixelFloat _castY = new SubpixelFloat();

#if DEBUG
        List<Vector2> _lastCast = new List<Vector2>();
#endif
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
            var mybounds = Bounds;
            mybounds.Location += offset;
            if (other is BoxCollider)
            {
                var otherbounds = (other as BoxCollider).Bounds;
                return CollisionChecks.RectAndRect(otherbounds, mybounds);
            }
            else if (other is GridCollider)
            {
                var grid = other as GridCollider;
                mybounds.Location -= other.Entity.Transform.Position;
                return CollisionChecks.RectAndGrid(mybounds, grid.Grid, grid.CellWidth, grid.CellHeight);
            }
            return false;
        }
        public bool Cast(Vector2 position, int mask)
        {
#if DEBUG
            _lastCast.Clear();
#endif
            _castX.Reset();
            _castX.Reset();
            var v = position - Entity.Transform.Position.ToVector2();
            float d = v.Length();
            v.Normalize();
            v.X *= Bounds.Width * 0.5f;
            v.Y *= Bounds.Height * 0.5f;
            Vector2 offset = Vector2.Zero;
            float amount = v.Length();
            for (float i = 0; i < d; i += amount)
            {
                var xs = _castX.Update(offset.X);
                var ys = _castY.Update(offset.Y);
                if (xs != 0 || ys != 0)
                {
#if DEBUG
                    _lastCast.Add(Bounds.Location.ToVector2() + new Vector2((float)xs, ys));
#endif

                    var collidery = Check(mask, new Point((int)xs, (int)ys));
                    if (collidery != null)
                    {
                        return false;
                    }
                }
                offset += v;
            }
            return true;
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
        public bool IsVisibleFromCamera(Camera camera)
        {
            return camera.Bounds.Intersects(Bounds);
        }
        public void DebugRender(Graphics graphics)
        {
            graphics.DrawHollowRect(Bounds, Color.Red);
            var rect = Bounds;
            for (var i = 0; i <  _lastCast.Count; i++)
            {
                graphics.DrawHollowRect(new Rectangle(_lastCast[i].ToPoint(), rect.Size), Color.Red);
            }
        }
        public void Render(Graphics graphics)
        {
        }
    }
}
