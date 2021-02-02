using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public interface IGrid
    {
        public int CellWidth { get; }
        public int CellHeight { get;}
        public int Width { get; }
        public int Height { get; }
        byte GetValue(int x, int y);
        byte GetValueByIndex(int idx);
        void SetValue(int x, int y, byte value);
        void SetValueByIndex(int index, byte value);
    }
    public class TilemapRenderer : Component, IGrid, IRenderable
    {
        public int Layer { get; set; }
        public int CellWidth => Tileset.TileWidth;
        public int CellHeight => Tileset.TileHeight;
        public static byte EmptyTile = 0;
        public int Width => _width_in_tiles;
        public int Height => _height_in_tiles;
        public Tileset Tileset { get; set; }
        byte[] _grid;
        int _width_in_tiles;
        int _height_in_tiles;
        public TilemapRenderer(int wtiles, int htiles, Tileset tileset)
        {
            _width_in_tiles = wtiles;
            _height_in_tiles = htiles;
            _grid = new byte[_width_in_tiles * _height_in_tiles];
            Tileset = tileset;
        }
        public override void CleanUp()
        {
        }
        public byte GetValue(int x, int y)
        {
            var idx = y * _width_in_tiles + x;
            return GetValueByIndex(idx);
        }
        public byte GetValueByIndex(int idx)
        {
            if (idx >= _grid.Length || idx < 0) return 0;
            return _grid[idx];
        }
        public bool IsVisibleFromCamera(Camera camera)
        {
            return true;
        }
        public void Render(Graphics graphics)
        {
            if (Tileset == null) return;
            var gfx = Engine.Instance.Graphics;
            var bounds = graphics.Camera.Bounds;
            bounds.Location -= Entity.Transform.Position;
            int left = Math.Clamp( (int)Math.Floor((float)bounds.Left / Tileset.TileWidth), 0, Width);
            int right = Math.Clamp( (int)Math.Floor((float)bounds.Right / Tileset.TileWidth), 0, Width);
            int top = Math.Clamp((int)Math.Floor((float)bounds.Top / Tileset.TileHeight), 0, Height);
            int bottom = Math.Clamp((int)Math.Floor((float)bounds.Bottom / Tileset.TileHeight), 0, Height);
            for (var j = top; j <= bottom; j++)
            {
                for (var i = left; i <= right; i++)
                {
                    var tile_id = GetValue(i, j);
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
            var idx = y * _width_in_tiles + x;
            SetValueByIndex(idx, value);
        }
        public void SetValueByIndex(int index, byte value)
        {
            _grid[index] = value;
        }
    }
}
