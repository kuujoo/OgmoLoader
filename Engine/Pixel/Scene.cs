using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{

    public class Scene : IDisposable
    {
        public Color ClearColor { get; set; }
        public Surface ApplicationSurface { get; set; }
        public RuntimeContentManager Content { get; private set; }
        public bool Paused;
        List<Camera> _cameras = new List<Camera>();
        List<SceneComponent> _sceneComponents = new List<SceneComponent>();
        Rectangle _finalDestinationRect;
        List<Layer> _layers = new List<Layer>();
        public Scene(int game_width, int game_height)
        {
            ClearColor = Color.Aquamarine;
            Content = new RuntimeContentManager();
            ApplicationSurface = new Surface(game_width, game_height);
            UpdateDrawRect();
            Initialize();
        }
        public virtual void Initialize()
        {

        }
        public EntityLayer CreateEntityLayer(int id, string name)
        {
            var layer = new EntityLayer(this, id);
            layer.Name = name;
            _layers.Add(layer);
            return layer;
        }
        public TileLayer CreateTileLayer(int id, string name, int width, int height, Tileset tileset)
        {
            var layer = new TileLayer(this, id, width, height, tileset);
            layer.Name = name;
            _layers.Add(layer);
            return layer;
        }
        public T GetLayer<T>(int id) where T: Layer
        {
            for (var i = 0; i < _layers.Count; i++) {
                if(_layers[i].Id == id)
                {
                    return _layers[i] as T;
                }
            }
            return null;
        }
        public SceneComponent AddSceneComponent(SceneComponent sceneComponent)
        {
            sceneComponent.Scene = this;
            sceneComponent.Initialize();
            _sceneComponents.Add(sceneComponent);
            return sceneComponent;
        }
        public T GetSceneComponent<T>() where T : SceneComponent
        {
            for (var i = 0; i < _sceneComponents.Count; i++)
            {
                if (_sceneComponents[i] is T)
                {
                    return _sceneComponents[i] as T;
                }
            }
            return null;
        }
        public void AddCamera(Camera camera)
        {
            if (camera.Surface == null)
            {
                camera.Surface = ApplicationSurface;
            }
            _cameras.Add(camera);
        }
        public void ResizeApplicationSurface(int game_width, int game_height)
        {
            ApplicationSurface.Resize(game_width, game_height);
        }
        public Entity CreateEntity(int layerid)
        {
            for (var i = 0; i < _layers.Count; i++)
            {
                if (_layers[i].Id == layerid)
                {
                    var entity_layer = _layers[i] as EntityLayer;
                    if (entity_layer != null)
                    {
                        var entity = new Entity();
                        entity_layer.Entities.AddEntity(entity);
                        return entity;
                    }
                }
            }
            return null;
        }
        public Entity AddEntity(Entity entity, int layerid)
        {
            for (var i = 0; i < _layers.Count; i++)
            {
                if (_layers[i].Id == layerid)
                {
                    var entity_layer = _layers[i] as EntityLayer;
                    if (entity_layer != null)
                    {
                        entity_layer.Entities.AddEntity(entity);
                        return entity;
                    }
                }
            }
            return null;
        }
        public void DestroyEntity(Entity entity)
        {
            for (var i = 0; i < _layers.Count; i++)
            {
                var entity_layer = _layers[i] as EntityLayer;
                if (entity_layer != null)
                {
                    entity.Destroy();
                    entity.CleanUp();
                    entity_layer.Entities.Remove(entity);
                }
            }
        }
        public void RemoveSceneComponent(SceneComponent component)
        {
            _sceneComponents.Remove(component);
            component.RemovedFromScene();
        }
        public T CreateEntity<T>(int layerid) where T : Entity, new()
        {
            for (var i = 0; i < _layers.Count; i++)
            {
                if (_layers[i].Id == layerid)
                {
                    var entity_layer = _layers[i] as EntityLayer;
                    if (entity_layer != null)
                    {
                        var entity = new T();
                        entity_layer.Entities.AddEntity(entity);
                        return entity;
                    }
                }
            }
            return null;
        }
        public Entity AddEntity(int layerid, Entity entity)
        {
            for (var i = 0; i < _layers.Count; i++)
            {
                if (_layers[i].Id == layerid)
                {
                    var entity_layer = _layers[i] as EntityLayer;
                    if (entity_layer != null)
                    {
                        entity_layer.Entities.AddEntity(entity);
                        return entity;
                    }
                }
            }
            return null;
        }
        public void BeginScene()
        {
        }
        public void EndScene()
        {
            for (var i = 0; i < _sceneComponents.Count; i++)
            {
                _sceneComponents[i].CleanUp();
            }
            _sceneComponents.Clear();

            for (var i = 0; i < _layers.Count; i++)
            {
                _layers[i].CleanUp();
            }
            _layers.Clear();
        }
        public virtual void Update()
        {
            if (Paused) return;
            for (var i = 0; i < _sceneComponents.Count; i++)
            {
                _sceneComponents[i].Update();
            }
            for (var i = 0; i < _layers.Count; i++)
            {
                _layers[i].Update();
            }
        }
        public void OnGraphicsDeviceReset()
        {
            for (var i = 0; i < _layers.Count; i++)
            {
                _layers[i].OnGraphicsDeviceReset();
            }

            UpdateDrawRect();
        }
        void UpdateDrawRect()
        {

            var sw = Screen.Width;
            var sh = Screen.Height;
            var saspect = (float)sw / (float)sh;

            var surface_w = ApplicationSurface.Target.Width;
            var surface_h = ApplicationSurface.Target.Height;


            int scale = 1;
            if ((float)surface_w / (float)surface_h > saspect)
            {
                scale = sw / surface_w;
            }
            else
            {
                scale = sh / surface_h;
            }

            _finalDestinationRect.Width = (int)Math.Ceiling((double)surface_w * scale);
            _finalDestinationRect.Height = (int)Math.Ceiling((double)surface_h * scale);
            _finalDestinationRect.X = (sw - _finalDestinationRect.Width) / 2;
            _finalDestinationRect.Y = (sh - _finalDestinationRect.Height) / 2;
        }
        public void Render()
        {
            // FIXME: sort only when needed
            _cameras.Sort();
            _layers.Sort();

            var gfx = Engine.Instance.Graphics;
            for (var i = 0; i < _cameras.Count; i++)
            {
                if (_cameras[i].Enabled)
                {
                    gfx.Begin(_cameras[i]);
                    for (var j = 0; j < _layers.Count; j++)
                    {
                        if (_cameras[i].CanRenderLayer(_layers[j]))
                        {
                            _layers[j].Render(_cameras[i]);
                        }
                    }
                    gfx.End();
                }
            }

            // Final render

            gfx.Begin(null);
            gfx.Device.Clear(Color.Black);
            gfx.SpriteBatch.Draw(ApplicationSurface.Target, _finalDestinationRect, Color.White);
            gfx.End();
        }

        void Dispose(bool dispose)
        {
            if (dispose)
            {
                ApplicationSurface.Dispose();
                Content.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
