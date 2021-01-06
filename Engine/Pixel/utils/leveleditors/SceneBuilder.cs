using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public abstract class SceneBuilder
    {
        protected Scene _scene;
        int _entityDepth = 0;
        TilemapRenderer _activeTilemap = null;
        Dictionary<string, Tileset> _tilesets = new Dictionary<string, Tileset>();
        public SceneBuilder(Scene scene)
        {
            _scene = scene;
        }
        public void AddTileset(string key, Tileset tileset)
        {
            _tilesets[key] = tileset;
        }
        public abstract void Build();

        protected void BeginTileLayer(int id, string name, int width, int height, Tileset tileset)
        {
            var t = _scene.CreateEntity(id);        
            _activeTilemap = t.AddComponent(new TilemapRenderer(width, height, tileset));
        }
        protected Tileset GetTileset(string tileset_name)
        {
            return _tilesets[tileset_name];
        }
        protected void SetTile(int index, byte tile)
        {
            _activeTilemap.SetValueByIndex(index, tile);
        }
        protected void SetTile(int x, int y, byte tile)
        {
            _activeTilemap.SetValue(x, y, tile);
        }
        protected void BeginEntityLayer(int id, string name)
        {
            _entityDepth = id;
        }
        protected Entity CreateEntity(string entity, Settings injectsettings)
        {
            var e = _scene.CreateEntity(_entityDepth);
            var component = Engine.Instance.Reflection.BuildComponent(entity);
            e.AddComponent(injectsettings);
            e.AddComponent(component);
            return _scene.AddEntity(e, _entityDepth);        }
        protected void EndLayer()
        {
            _entityDepth = 0;
        }
    }
}
