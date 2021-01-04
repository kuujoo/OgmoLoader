﻿using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public abstract class SceneBuilder
    {
        protected Scene _scene;
        TileLayer _activeTileLayer = null;
        EntityLayer _activeEntityLayer = null;
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
            var layer = _scene.GetLayer<TileLayer>(id);
            if (layer != null)
            {
                if (layer.Width == width && layer.Height == height && layer.Tileset == tileset)
                {
                    _activeTileLayer = layer;
                }
            }
            else
            {
                _activeTileLayer = _scene.CreateTileLayer(id, name, width, height, tileset);
            }
        }
        protected Tileset GetTileset(string tileset_name)
        {
            return _tilesets[tileset_name];
        }
        protected void SetTile(int index, byte tile)
        {
            _activeTileLayer.SetValueByIndex(index, tile);
        }
        protected void SetTile(int x, int y, byte tile)
        {
            _activeTileLayer.SetValue(x, y, tile);
        }
        protected void BeginEntityLayer(int id, string name)
        {
            var layer = _scene.GetLayer<EntityLayer>(id);
            if (layer != null)
            {
                _activeEntityLayer = layer;
            }
            else
            {
                _activeEntityLayer = _scene.CreateEntityLayer(id, name);
            }
        }
        protected Entity CreateEntity(string entity, SettingsComponent injectsettings)
        {
            var e = Engine.Instance.Reflection.BuildEntity(entity);
            e.AddComponent(injectsettings);
            _scene.AddEntity(e, _activeEntityLayer.Id);
            return e;
        }
        protected void EndLayer()
        {
            _activeEntityLayer = null;
            _activeTileLayer = null;
        }
    }
}