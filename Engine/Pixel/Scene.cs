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
        public Effect FinalEffect { get; set; }
        public Surface ApplicationSurface { get; set; }
        List<Camera> _cameras = new List<Camera>();
        List<SceneComponent> _sceneComponents = new List<SceneComponent>();
        protected Rectangle finalDestinationRect;
        Tracker _tracker;
        public Scene(int game_width, int game_height)
        {
            ApplicationSurface = new Surface(game_width, game_height);
            Entities = new EntityList();
            DebugRender = false;
            UpdateDrawRect();
            _tracker = AddSceneComponent(new Tracker());
            Initialize();
        }
        public virtual void Initialize()
        {
          
        }
        public void EndScene()
        {
            for (var i = 0; i < Entities.Count; i++)
            {
                DestroyEntity(Entities[i]);
            }

            for (var i = 0; i < _sceneComponents.Count; i++)
            {
                _sceneComponents[i].CleanUp();
            }
            _sceneComponents.Clear();
        }
        public virtual void Update()
        {
            for (var i = 0; i < _sceneComponents.Count; i++)
            {
                _sceneComponents[i].Update();
            }
            Entities.UpdateLists();
            for (var i = 0; i < Entities.Count; i++)
            {
                Entities[i].Components.UpdateLists();
            }

            var updateables = _tracker.GetUpdateables();
            for(var i = 0; i < updateables.Count; i++)
            {
                if (updateables[i].Enabled)
                {
                    updateables[i].Update();
                }
            }
        }
        public void OnGraphicsDeviceReset()
        {
            UpdateDrawRect();
        }
        public void Render()
        {
            _cameras.Sort();
            var gfx = Engine.Instance.Graphics;
            var renderables = _tracker.GetRenderables();
            for (var i = 0; i < _cameras.Count; i++)
            {
                if (_cameras[i].Enabled)
                {
                    _cameras[i].BeginRender(gfx);
                    for(var r = 0; r < renderables.Count; r++)
                    {
                        var renderable = renderables[r];
                        if(renderable.Enabled)
                        {
                            _cameras[i].Render(gfx, renderable);
                        }
                    }
#if DEBUG
                    if (DebugRender)
                    { 
                        for (var r = 0; r < renderables.Count; r++)
                        {
                            var renderable = renderables[r];
                            if (renderable.Enabled)
                            {
                                _cameras[i].DebugRender(gfx, renderable);
                            }
                        }
                    }
#endif
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
            gfx.PushSamplerState(SamplerState.PointClamp);
            gfx.Begin();
            gfx.Device.Clear(Color.Black);
            gfx.SpriteBatch.Draw(ApplicationSurface.Target, finalDestinationRect, Color.White);
            gfx.End();
            if (FinalEffect != null)
            {
                gfx.PopEffect(FinalEffect);   
            }
            gfx.PopSamplerState();
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
        public Entity CreateEntity()
        {
            return AddEntity(new Entity());
        }
        public Entity AddEntity(Entity entity)
        {
            entity.Scene = this;
            Entities.Add(entity);
            return entity;
        }
        public void DestroyEntity(Entity entity)
        {
            if (Entities.Contains(entity))
            {
                entity.RemoveComponents();
                Entities.Remove(entity);
            }
        }
        void UpdateDrawRect()
        {

            var sw = Engine.Instance.Screen.Width;
            var sh = Engine.Instance.Screen.Height;
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
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
