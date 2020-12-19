namespace kuujoo.Alla
{
    public class LdtkGridTile
    {
        public int[] Px { get; set; }
        public int[] Src { get; set; }
        public int F { get; set; }
        public int T { get; set; }
        public int[] D { get; set; }
    }
    public class LdtkEntityInstance
    {
        public int DefUid { get; set; }
        public int[] Px { get; set; }
    }
    public class LdtkLayerInstance
    {
        public int LevelId { get; set; }
        public int LayerDefUid { get; set;}
        public int pxOffsetX { get; set; }
        public int pxOffsetY { get; set; }
        public int[] IntGrid { get; set; }
        public LdtkGridTile[] GridTiles {get;set;}
        public LdtkEntityInstance[] EntityInstances { get; set; }
    }
}
