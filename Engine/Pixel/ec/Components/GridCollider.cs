using Microsoft.Xna.Framework;
using System;

namespace kuujoo.Pixel
{
    public class GridCollider : Collider, IRenderable
    {
        public ByteGrid Grid => _grid;
        public int Layer { get; set; }
        public int CellWidth { get; private set; }
        public int CellHeight { get; private set; }
        public override Rectangle Bounds => new Rectangle((Entity.Transform.Position), new Point(Width * CellWidth, Height * CellHeight));
        public int Width => _grid == null ? 0 : _grid.Width;
        public int Height => _grid == null ? 0 : _grid.Height;
        ByteGrid _grid;
        public void Set(ByteGrid grid, int cellw, int cellh)
        {
            _grid = grid;
            CellWidth = cellw;
            CellHeight = cellh;
            Updated?.Invoke(this);
        }
        public override bool Collides(Collider other, Point offset)
        {
            if (_grid == null) return false;

            if (other is BoxCollider)
            {
                var r = other.Bounds;
                r.Location -= (Entity.Transform.Position + offset);
                return CollisionChecks.RectAndGrid(r, _grid, CellWidth, CellHeight);            
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
            if (_grid == null) return;

            var bounds = graphics.Camera.Bounds;
            bounds.Location -= Entity.Transform.Position;
            int left = Math.Clamp((int)Math.Floor((float)bounds.Left / CellWidth), 0, Width - 1);
            int right = Math.Clamp((int)Math.Floor((float)bounds.Right / CellWidth), 0, Width - 1);
            int top = Math.Clamp((int)Math.Floor((float)bounds.Top / CellHeight), 0, Height - 1);
            int bottom = Math.Clamp((int)Math.Floor((float)bounds.Bottom / CellHeight), 0, Height - 1);
            for (var j = top; j <= bottom; j++)
            {
                for (var i = left; i <= right; i++)
                {
                    if(_grid.GetValue(i, j) != 0)
                    {
                        graphics.DrawHollowRect(new Rectangle(Entity.Transform.Position.X + i * CellWidth, Entity.Transform.Position.Y + j * CellHeight, CellWidth, CellHeight), Color.Red);
                    }
                }
            }
        }
    }
}
