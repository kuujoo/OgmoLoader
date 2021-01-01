namespace kuujoo.Pixel
{
    public class Tilegrid 
    {
        public int Width { get; }
        public int Height { get; }
        int[] _tiles;
        public Tilegrid(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new int[Width * Height];
            for(var i = 0; i < Width * Height; i++)
            {
                _tiles[i] = -1;
            }
        }
        public int getTileId(int x, int y)
        {
            return _tiles[y * Width + x];
        }
        public void SetTileId(int x, int y, int tileid)
        {
            _tiles[y * Width + x] = tileid;
        }
        public void SetTileId(int index, int tileid)
        {
            _tiles[index] = tileid;
        }
    }
}
