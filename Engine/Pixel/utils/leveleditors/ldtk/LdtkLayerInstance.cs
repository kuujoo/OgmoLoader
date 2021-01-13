using System.Text.Json;

namespace kuujoo.Pixel
{
    public class LdtkGridTile
    {
        public int[] Px { get; set; }
        public int[] Src { get; set; }
        public int F { get; set; }
        public int T { get; set; }
        public int[] D { get; set; }
    }

    public class LdtkRealEditorvalue
    {
        public string Id { get; set; }
        public JsonElement[] Params { get; set; }
        public bool GetParamAsBool(int index = 0)
        {
            return Params[index].GetBoolean();
        }
        public int[] GetParamsAsPoint(int index = 0)
        {
            var str = Params[index].GetString();
            var splitted = str.Split(",");
            int[] r = { int.Parse(splitted[0]), int.Parse(splitted[1]) };
            return r;
        }
        public string GetParamAsString(int index = 0)
        {
            return Params[index].GetString();
        }
        public int GetParamAsInt(int index = 0)
        {
            return Params[index].GetInt32();
        }
    }
    public class LdtkFieldInstance
    {
        public string __identifier { get; set; }
        public string __type { get; set; }
        public dynamic __value { get; set; }
        public int DefUid { get; set; }
        public LdtkRealEditorvalue[] RealEditorValues { get; set; }
    }
    public class LdtkEntityInstance
    {
        public string __identifier { get; set; }
        public int[] _grid { get; set; }
        public int DefUid { get; set; }
        public int[] Px { get; set; }
        public LdtkFieldInstance[] FieldInstances { get; set; }
    }
    public class LdtkLayerInstance
    {

        public string __identifier { get; set; }
        public string __type { get; set; }
        public int __cWid { get; set; }
        public int __cHei { get; set; }
        public int __gridSize { get; set; }
        public float __opacity { get; set; }
        public int __pxTotalOffsetX { get; set; }
        public int __pxTotalOffsetY { get; set; }
        public int? __tilesetDefUid { get; set; }
        public string __tilesetRelPath { get; set; }
        public int LevelId { get; set; }
        public int LayerDefUid { get; set;}
        public int pxOffsetX { get; set; }
        public int pxOffsetY { get; set; }
        public int[] IntGrid { get; set; }
        public LdtkGridTile[] GridTiles {get;set;}
        public LdtkEntityInstance[] EntityInstances { get; set; }
    }
}
