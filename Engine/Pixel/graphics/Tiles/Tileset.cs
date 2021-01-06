using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kuujoo.Pixel
{
    public class Tile
    {
        public Texture2D Texture {get; private set;}
        public Rectangle Bounds { get; private set; }
        public Tile(Texture2D texture, Rectangle bounds)
        {
            Texture = texture;
            Bounds = bounds;
        }
    }
    public class Tileset
    {
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        Texture2D _texture;
        Rectangle _rect;
        Tile[] _tiles;
        public Tileset(int tilew, int tileh, Texture2D texture, Rectangle rect)
        {
            TileWidth = tilew;
            TileHeight = tileh;
            _rect = rect;
            _texture = texture;
            generateTiles();
        }
        private void generateTiles()
        {
            var _tiles_x = _rect.Width / TileWidth;
            var _tiles_y = _rect.Height / TileHeight;
            _tiles = new Tile[_tiles_x * _tiles_y];
            for (var j = 0; j < _tiles_y; j++)
            {
                for (var i = 0; i < _tiles_x; i++)
                {
                    var p = j * _tiles_x + i;

                    _tiles[p] = new Tile(_texture, new Rectangle(_rect.X + i * TileWidth, _rect.Y + j * TileHeight, TileWidth, TileHeight));
                }
            }
        }
        public Tile GetTile(int id)
        {
            return _tiles[id];
        }
    }

}
