namespace kuujoo.Alla
{
    public class LdtkDefines
    {
        public LdtkLayerDefine[] Layers { get; set; }
        public LdtkTilesetDefine[] Tilesets { get; set; }
        public LdtkEntityDefine[] Entities { get; set; }
        public LdtkLayerDefine GetLayerDefineByUid(int uid)
        {
            for(var i = 0; i < Layers.Length; i++)
            {
                if(Layers[i].Uid == uid)
                {
                    return Layers[i];
                }
            }
            return null;
        }
        public LdtkTilesetDefine GetTilesetsDefineByUid(int uid)
        {
            for (var i = 0; i < Tilesets.Length; i++)
            {
                if (Tilesets[i].Uid == uid)
                {
                    return Tilesets[i];
                }
            }
            return null;
        }
        public LdtkEntityDefine GetEntityDefineByUid(int uid)
        {
            for (var i = 0; i < Entities.Length; i++)
            {
                if (Entities[i].Uid == uid)
                {
                    return Entities[i];
                }
            }
            return null;
        }
    }
    public class LdtkLayerDefine
    {
        public string Identifier { get; set; }
        public string Type { get; set; }
        public int Uid { get; set; }
        public int GridSize { get; set; }
        public int? TilesetDefUid { get; set; }
    }
    public class LdtkTilesetDefine
    {
        public string Identifier { get; set; }
        public int Uid { get; set; }
        public string RelPath { get; set; }
        public int PxWid { get; set; }
        public int PxHeight { get; set; }
        public int TileGridSize { get; set; }
    }
    public class LdtkEntityDefine
    {
        public string Identifier { get; set; }
        public int Uid { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string RenderMode { get; set; }
        public int? TilesetId { get; set; }
        public int? TileId { get; set; }
        public string TileRenderMode { get; set; }
        public float PivotX { get; set; }
        public float PivotY { get; set; }
    }
}
