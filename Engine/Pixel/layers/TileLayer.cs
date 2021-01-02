using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public interface IGrid
    {
        public int Width { get; }
        public int Height { get; }
        byte GetValue(int x, int y);
        byte GetValueByIndex(int idx);
        void SetValue(int x, int y, byte value);
        void SetValueByIndex(int index, byte value);
    }
    public class TileLayer : Layer, IGrid
    {
        public static byte EmptyTile = 0;
        public int Width => _width_in_tiles;
        public int Height => _height_in_tiles;
        public Tileset Tileset { get; set; }
        byte[] _grid;
        int _width_in_tiles;
        int _height_in_tiles;
        Scene _scene;
        public TileLayer(Scene scene, int id, int wtiles, int htiles, Tileset tileset)
        {
            Id = id;
            _scene = scene;
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
            return _grid[idx];
        }

        public override void OnGraphicsDeviceReset()
        {
        }

        public override void Render(Camera camera)
        {
            if (Tileset == null) return;
            var gfx = Engine.Instance.Graphics;
            var bounds = camera.Bounds;
            int left = (int)Math.Floor( (float)bounds.Left / Tileset.TileWidth);
            int right = (int)Math.Ceiling((float) bounds.Right / Tileset.TileWidth);
            int top = bounds.Top / Tileset.TileHeight;
            int bottom = bounds.Bottom / Tileset.TileHeight;
            for(var j = top; j <= bottom; j++)
            {
                for (var i = left; i <= right; i++)
                {
                    var tile_id = GetValue(i, j);
                    if(tile_id != EmptyTile)
                    {
                        var tile = Tileset.GetTile(tile_id);
                        gfx.SpriteBatch.Draw(tile.Texture, new Vector2(i * Tileset.TileWidth, j * Tileset.TileHeight), tile.Bounds, Color.White);
                    }
                }
            }
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

        public override void Update()
        {
        }
    }
}
