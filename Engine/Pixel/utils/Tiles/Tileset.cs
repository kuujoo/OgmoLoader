using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace kuujoo.Pixel
{
    public class Tileset
    {
        public int CellWidth { get; private set; }
        public int CellHeight { get; private set; }
        Texture2D _texture;
        Rectangle _rect;
        Tile[] _tiles;
        public Tileset(int cellw, int cellh, Texture2D texture, Rectangle rect)
        {
            CellWidth = cellw;
            CellHeight = cellh;
            _rect = rect;
            _texture = texture;
            generateTiles();
        }
        private void generateTiles()
        {
            var _tiles_x = _rect.Width / CellWidth;
            var _tiles_y = _rect.Height / CellHeight;
            _tiles = new Tile[_tiles_x * _tiles_y];
            for (var j = 0; j < _tiles_y; j++)
            {
                for (var i = 0; i < _tiles_x; i++)
                {
                    var p = j * _tiles_x + i;

                    _tiles[p] = new Tile(_texture, new Rectangle(_rect.X + i * CellWidth, _rect.Y + j * CellHeight, CellWidth, CellHeight));
                }
            }
        }
        public Tile GetTile(int id)
        {
            return _tiles[id];
        }
    }

}