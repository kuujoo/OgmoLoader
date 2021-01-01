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
        public PixelContentManager Content { get; private set; }
        public bool Paused;
        public EntityList Entities;
        List<Camera> _cameras = new List<Camera>();
        List<SceneComponent> _sceneComponents = new List<SceneComponent>();
        Rectangle _finalDestinationRect;
        public Scene(int game_width, int game_height)
        {
            ClearColor = Color.Aquamarine;
            Content = new PixelContentManager();
            Entities = new EntityList(this);
            ApplicationSurface = new Surface(game_width, game_height);
            UpdateDrawRect();
            Initialize();
;       }
        public virtual void Initialize()
        {

        }
        public void AddSceneComponent(SceneComponent sceneComponent)
        {
            sceneComponent.Scene = this;
            sceneComponent.Initialize();
            _sceneComponents.Add(sceneComponent);
        }
        public T GetSceneComponent<T>() where T : SceneComponent
        {
            for(var i = 0; i < _sceneComponents.Count; i++)
            {
                if(_sceneComponents[i] is T)
                {
                    return _sceneComponents[i] as T;
                }
            }
            return null;
        }
        public void AddCamera(Camera camera)
        {
            if(camera.Surface == null)
            {
                camera.Surface = ApplicationSurface;
            }
            _cameras.Add(camera);
        }
        public void ResizeApplicationSurface(int game_width, int game_height)
        {
            ApplicationSurface.Resize(game_width, game_height);
        }
        public Entity CreateEntity()
        {
            var entity = new Entity();
            Entities.AddEntity(entity);
            return entity;
        }
        public void DestroyEntity(Entity entity)
        {
            Entities.Remove(entity);
        }
        public void RemoveSceneComponent(SceneComponent component)
        {
            _sceneComponents.Remove(component);
            component.RemovedFromScene();
        }
        public T CreateEntity<T>() where T : Entity, new()
        {
            var entity = new T();
            Entities.AddEntity(entity);
            return entity;
        }
        public Entity AddEntity(Entity entity)
        {
            Entities.AddEntity(entity);
            return entity;
        }
        public void BeginScene()
        {
        }
        public void EndScene()
        {
            for(var i = 0; i < _sceneComponents.Count; i++)
            {
                _sceneComponents[i].Destroy();
            }
            _sceneComponents.Clear();
            Entities.RemoveAll();
        }
        public virtual void Update ()
        {
            if (Paused) return;
            Entities.UpdateLists();
            for(var i = 0; i < _sceneComponents.Count; i++)
            {
                _sceneComponents[i].Update();
            }

            for (var i = 0; i < Entities.Count; i++)
            {
                Entities[i].Update(); ;
            }

        }
        public void OnGraphicsDeviceReset()
        {
            for (var i = 0; i < Entities.Count; i++)
            {
                Entities[i].OnGraphicsDeviceReset();
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
            if( (float)surface_w / (float)surface_h > saspect )
            {
                scale = sw / surface_w;
            } 
            else
            {
                scale = sh / surface_h;
            }

            _finalDestinationRect.Width = (int) Math.Ceiling( (double)surface_w * scale );
            _finalDestinationRect.Height = (int) Math.Ceiling( (double) surface_h * scale);
            _finalDestinationRect.X = (sw - _finalDestinationRect.Width) / 2;
            _finalDestinationRect.Y = (sh - _finalDestinationRect.Height) / 2;
        }
        public void Render()
        {
            _cameras.Sort();
            for (var i = 0; i < _cameras.Count; i++)
            {
                _cameras[i].Render(this);
            }

            var gfx = Engine.Instance.Graphics;
            gfx.Begin(null);
            gfx.Device.Clear(Color.Black);
            gfx.SpriteBatch.Draw(ApplicationSurface.Target, _finalDestinationRect, Color.White);
            gfx.End();
        }

        void Dispose(bool dispose)
        {
            if(dispose)
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
