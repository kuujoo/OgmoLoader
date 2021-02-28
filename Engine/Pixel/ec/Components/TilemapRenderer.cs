using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public class TilemapRenderer : Component, IRenderable
    {
        public int Layer { get; set; }
        public static byte EmptyTile = 0;
        public int Width => _grid.Width;
        public int Height => _grid.Height;
        public Tileset Tileset { get; set; }
        ByteGrid _grid;
        public TilemapRenderer(int wtiles, int htiles, Tileset tileset)
        {
            _grid = new ByteGrid(wtiles, htiles);
            Tileset = tileset;
        }
        public TilemapRenderer(ByteGrid grid, Tileset tileset)
        {
            _grid = grid;
            Tileset = tileset;
        }
        public override void CleanUp()
        {
        }
        public byte GetValue(int x, int y)
        {
            return _grid.GetValue(x, y);
        }
        public byte GetValueByIndex(int idx)
        {
            return _grid.GetValueByIndex(idx);
        }
        public bool IsVisibleFromCamera(Camera camera)
        {
            return _grid != null && Tileset != null;
        }
        public void SetGrid(ByteGrid grid)
        {
            _grid = grid;
        }
        public void Render(Graphics graphics)
        {
            if (Tileset == null) return;
            if (_grid == null) return;
            var gfx = Engine.Instance.Graphics;
            var bounds = graphics.Camera.Bounds;
            bounds.Location -= Entity.Transform.Position;
            int left = Math.Clamp( (int)Math.Floor((float)bounds.Left / Tileset.TileWidth), 0, Width - 1);
            int right = Math.Clamp( (int)Math.Floor((float)bounds.Right / Tileset.TileWidth), 0, Width - 1);
            int top = Math.Clamp((int)Math.Floor((float)bounds.Top / Tileset.TileHeight), 0, Height - 1);
            int bottom = Math.Clamp((int)Math.Floor((float)bounds.Bottom / Tileset.TileHeight), 0, Height - 1);
            for (var j = top; j <= bottom; j++)
            {
                for (var i = left; i <= right; i++)
                {
                    var tile_id = _grid.GetValue(i, j);
                    if (tile_id != EmptyTile)
                    {
                        var tile = Tileset.GetTile(tile_id);
                        gfx.SpriteBatch.Draw(tile.Texture, Entity.Transform.Position.ToVector2() + new Vector2(i * Tileset.TileWidth, j * Tileset.TileHeight), tile.Bounds, Color.White);
                    }
                }
            }
        }
        public void DebugRender(Graphics graphics)
        {
        }
        public void SetValue(int x, int y, byte value)
        {
            _grid.SetValue(x, y, value);
        }
        public void SetValueByIndex(int index, byte value)
        {
            _grid.SetValueByIndex(index, value);
        }
    }
}
