namespace kuujoo.Pixel
{
    public class SpriteResourcesTilesetProvider : ITilesetProvider
    {
        SpriteResources _resources;
        string _sheetPath;
        int _tileWidth;
        int _tileHeight;
        public SpriteResourcesTilesetProvider(SpriteResources resources, string sheetpath, int tilewidth, int tileheight)
        {
            _resources = resources;
            _sheetPath = sheetpath;
            _tileWidth = tilewidth;
            _tileHeight = tileheight;
        }
        public Tileset GetTileset(string tileset)
        {
            var sp = _resources.GetSprite(_sheetPath, tileset);
            return new Tileset(_tileWidth, _tileHeight, sp.Texture, sp.Bounds);
        }
    }
}
