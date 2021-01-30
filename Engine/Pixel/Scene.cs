using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public class Scene : IDisposable
    {
        public bool DebugRender { get; set; }
        public EntityList Entities { get; private set; }
        public Physics Physics { get; private set; }
        public Effect FinalEffect { get; set; }
        public Color ClearColor { get; set; }
        public Surface ApplicationSurface { get; set; }
        public RuntimeContentManager Content { get; private set; }
        public bool Paused;
        List<Camera> _cameras = new List<Camera>();
        List<SceneComponent> _sceneComponents = new List<SceneComponent>();
        protected Rectangle finalDestinationRect;
        public Scene(int game_width, int game_height)
        {
            ClearColor = Color.Aquamarine;
            Content = new RuntimeContentManager();
            Physics = new Physics();
            ApplicationSurface = new Surface(game_width, game_height);
            Entities = new EntityList();
            UpdateDrawRect();
            Initialize();
            DebugRender = false;
        }
        public virtual void Initialize()
        {

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


            for (var i = 0; i < Entities.Count; i++)
            {
                Entities[i].Components.CleanUp();
            }
        }
        public virtual void Update()
        {
            if (Paused) return;
            for (var i = 0; i < _sceneComponents.Count; i++)
            {
                _sceneComponents[i].Update();
            }

            Entities.UpdateLists();

            for (var i = 0; i < Entities.Count; i++)
            {
                Entities[i].Components.UpdateLists();
                Entities[i].Components.Update();
            }
        }
        public void OnGraphicsDeviceReset()
        {
            for(var i = 0; i < Entities.Count; i++)
            {
                Entities[i].Components.OnGraphicsDeviceReset();
            }
            UpdateDrawRect();
        }
        public T FindComponent<T>() where T: Component
        {
            return Entities.FindComponent<T>();
        }
        public List<T> FindComponents<T>() where T : class
        {
            return Entities.FindComponents<T>();
        }
        public void Render()
        {
            _cameras.Sort();
            var gfx = Engine.Instance.Graphics;
            for (var i = 0; i < _cameras.Count; i++)
            {
                if (_cameras[i].Enabled)
                {
                    _cameras[i].BeginRender(gfx);
                    for(var e = 0; e < Entities.Count; e++)
                    {
                        var entity = Entities[e];
                        for (var c = 0; c < entity.Components.Count; c++)
                        {
                            var renderable = entity.Components[c];
                            _cameras[i].Render(gfx, renderable);
                            if (DebugRender)
                            {
                                _cameras[i].DebugRender(gfx, renderable);
                            }
                        }
                    }
                    _cameras[i].EndRender(gfx);
                }
            }
            // Final render
            FinalRender(gfx);        
        }
        public virtual void FinalRender(Graphics gfx)
        {
            if (FinalEffect != null)
            {
                gfx.PushEffect(FinalEffect);
            }
            gfx.Begin();
            gfx.Device.Clear(Color.Black);
            gfx.SpriteBatch.Draw(ApplicationSurface.Target, finalDestinationRect, Color.White);
            gfx.End();
            if (FinalEffect != null)
            {
                gfx.PopEffect(FinalEffect);   
            }
        }
        public T AddSceneComponent<T>(T sceneComponent) where T: SceneComponent
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
        public Camera AddCamera(Camera camera)
        {
            if (camera.Surface == null)
            {
                camera.Surface = ApplicationSurface;
            }
            _cameras.Add(camera);
            return camera;
        }
        public Entity CreateEntity(int depth)
        {
            return AddEntity(new Entity(), depth);
        }
        public Entity AddEntity(Entity entity, int depth)
        {
            entity.Scene = this;
            entity.Depth = depth;
            Entities.Add(entity);
            return entity;
        }
        public void DestroyEntity(Entity entity)
        {
            entity.Components.Destroy();
            entity.Components.CleanUp();
            entity.Components.RemovedFromEntity();
            entity.Components.RemoveAll();
            Entities.Remove(entity);
        }
        void UpdateDrawRect()
        {

            var sw = Screen.Width;
            var sh = Screen.Height;
            var saspect = sw / (float)sh;

            var surface_w = ApplicationSurface.Target.Width;
            var surface_h = ApplicationSurface.Target.Height;


            int scale = 1;
            if (surface_w / surface_h > saspect)
            {
                scale = sw / surface_w;
            }
            else
            {
                scale = sh / surface_h;
            }

            finalDestinationRect.Width = (int)Math.Ceiling((double)surface_w * scale);
            finalDestinationRect.Height = (int)Math.Ceiling((double)surface_h * scale);
            finalDestinationRect.X = (sw - finalDestinationRect.Width) / 2;
            finalDestinationRect.Y = (sh - finalDestinationRect.Height) / 2;
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
