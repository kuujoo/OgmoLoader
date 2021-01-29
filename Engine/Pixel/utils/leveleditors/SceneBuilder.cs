using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public abstract class SceneBuilder
    {
        public Action<Entity> EntityBuilt;
        public List<Rectangle> RoomRects { get; private set; }
        protected Scene _scene;
        int _entityDepth = 0;
        TilemapRenderer _activeTilemap = null;
        Dictionary<string, Tileset> _tilesets = new Dictionary<string, Tileset>();
        Dictionary<string, int> _wantedGridColliders = new Dictionary<string, int>();

        Dictionary<string, TilemapRenderer> _tilemaps = new Dictionary<string, TilemapRenderer>();
        Dictionary<string, GridCollider> _gridColliders = new Dictionary<string, GridCollider>();
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
        public void AddGridCollider(string key, int mask)
        {
            _wantedGridColliders[key] = mask;
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
            if(!_tilemaps.TryGetValue(name, out _activeTilemap)) {
                var t = _scene.CreateEntity(id);
                t.Transform.SetPosition(offsetx, offsety);
                _activeTilemap = t.AddComponent(new TilemapRenderer(width, height, tileset));          
                _tilemaps[name] = _activeTilemap;
            }

            int mask;
            if (_wantedGridColliders.TryGetValue(name, out mask))
            {
                if (!_gridColliders.TryGetValue(name, out _activeGridCollider))
                {
                    _activeGridCollider = _activeTilemap.Entity.AddComponent(new GridCollider(width, height, tileset.TileWidth, tileset.TileHeight));
                    _activeGridCollider.Mask = mask;
                }
            }

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

            EntityBuilt?.Invoke(e);
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
