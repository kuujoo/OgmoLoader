using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public interface ITilemapBuilder
    {
        TilemapRenderer Build(int width, int height, Tileset tileset);
    }

    public abstract class SceneBuilder
    {
        public List<Rectangle> RoomRects { get; private set; }
        protected Scene _scene;
        int _entityDepth = 0;
        TilemapRenderer _activeTilemap = null;
        Dictionary<string, Tileset> _tilesets = new Dictionary<string, Tileset>();
        Dictionary<string, int> _gridColliders = new Dictionary<string, int>();
        Dictionary<string, ITilemapBuilder> _tilemapbuilders = new Dictionary<string, ITilemapBuilder>();
        GridCollider _activeGridCollider;
        public SceneBuilder(Scene scene)
        {
            _scene = scene;
            RoomRects = new List<Rectangle>();
        }
        public void AddTileset(string key, Tileset tileset)
        {
            _tilesets[key] = tileset;
        }
        public void AddTilemapBuilder(string key, ITilemapBuilder builder)
        {
            _tilemapbuilders[key] = builder;
        }
        public void AddGridCollider(string key, int mask)
        {
            _gridColliders[key] = mask;
        }
        public void AddRoomBounds(int x, int y, int w, int h)
        {
            RoomRects.Add(new Rectangle(x, y, w, h));
        }
        public abstract void Build();
        public abstract void BuildTiles();
        public abstract void BuildRoomAt(int x, int y);
        public abstract void BuildEntitiesInRoomAt(int x, int y);

        protected void BeginTileLayer(int id, string name, int width, int height, int offsetx, int offsety, Tileset tileset)
        {
            var t = _scene.CreateEntity(id);
            t.Transform.SetPosition(offsetx, offsety);
            if (_tilemapbuilders.ContainsKey(name))
            {
                _activeTilemap = t.AddComponent(_tilemapbuilders[name].Build(width, height, tileset));
            }
            else
            {
                _activeTilemap = t.AddComponent(new TilemapRenderer(width, height, tileset));
            }
            int mask;
            if(_gridColliders.TryGetValue(name, out mask))
            {
                _activeGridCollider = t.AddComponent(new GridCollider(width, height, tileset.TileWidth, tileset.TileHeight));
                _activeGridCollider.Mask = mask;
            }
        }
        protected void BeginRoom(int x, int y, int width, int height)
        {
        }
        protected void EndRoom()
        {

        }
        protected Tileset GetTileset(string tileset_name)
        {
            return _tilesets[tileset_name];
        }
        protected void SetTile(int index, byte tile)
        {
            _activeTilemap.SetValueByIndex(index, tile);
            if(_activeGridCollider != null)
            {
                _activeGridCollider.SetValueByIndex(index, 255);
            }
        }
        protected void SetTile(int x, int y, byte tile)
        {
            SetTile(y * _activeTilemap.Width + x, tile);
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
            return e;
        }
        protected void EndLayer()
        {
            _entityDepth = 0;
            _activeGridCollider = null;
            _activeTilemap = null;
        }
    }
}
