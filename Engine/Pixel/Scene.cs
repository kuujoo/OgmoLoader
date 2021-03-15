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
        public Surface ApplicationSurface { get; set; }
        List<Camera> _cameras = new List<Camera>();
        List<SceneComponent> _sceneComponents = new List<SceneComponent>();
        List<PostProcessor> _postProcessors = new List<PostProcessor>();
        List<Entity> _entities = new List<Entity>();
        HashSet<Component> _recycleComponents = new HashSet<Component>();
        HashSet<Entity> _recycleEntities = new HashSet<Entity>();
        protected Rectangle finalDestinationRect;
        Tracker _tracker;
        ScenePool _pool;
        public Scene(int game_width, int game_height)
        {
            ApplicationSurface = new Surface(game_width, game_height, Color.Black);
            DebugRender = false;
            UpdateDrawRect();
            _tracker = AddSceneComponent(new Tracker());
            _pool = AddSceneComponent(new ScenePool());
            Initialize();
        }
        public virtual void Initialize()
        {
          
        }
        public T Get<T>() where T : Component, new()
        {
            return _pool.Get<T>();
        }
        public void Recycle(Component component)
        {
            component.Entity = null;
            _pool.Free(component);
        }
        public void Recycle(Entity entity)
        {
            _pool.Free(entity);
        }
        public void EndScene()
        {
            for (var i = 0; i < _entities.Count; i++)
            {
                DestroyEntity(_entities[i]);
            }
            _entities.Clear();

            for (var i = 0; i < _sceneComponents.Count; i++)
            {
                _sceneComponents[i].CleanUp();
            }
            _sceneComponents.Clear();

            foreach (var c in _recycleComponents)
            {
                Recycle(c);
            }
            foreach (var e in _recycleEntities)
            {
                Recycle(e);
            }
            _recycleComponents.Clear();
            _pool.Clear();
            CleanUp();
        }
        public virtual void CleanUp()
        {

        }
        public void AddPostProcessor(PostProcessor processor)
        {
            processor.Scene = this;
            processor.Initialize();
            _postProcessors.Add(processor);
        }
        public virtual void Update()
        {
            for (var i = 0; i < _sceneComponents.Count; i++)
            {
                _sceneComponents[i].Update();
            }

            foreach(var c in _recycleComponents)
            {
                c.Entity = null;
                Recycle(c);
            }
            foreach(var e in _recycleEntities)
            {
                Recycle(e);
            }
            _recycleComponents.Clear();
            _recycleEntities.Clear();

            var updateables = _tracker.GetUpdateables();
            for(var i = 0; i < updateables.Count; i++)
            {
                if (updateables[i].Enabled)
                {
                    updateables[i].Update();
                }
            }
        }
        public virtual void OnGraphicsDeviceReset()
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
            if (_postProcessors.Count > 0)
            {
                Surface tmp = ApplicationSurface;
                for (var i = 0; i < _postProcessors.Count; i++)
                {
                    tmp = _postProcessors[i].Process(gfx, tmp, finalDestinationRect);
                    if (tmp == null)
                    {
                        break;
                    }
                }
            }
            else
            {
                gfx.PushSamplerState(SamplerState.PointClamp);
                gfx.PushBlendState(BlendState.AlphaBlend);
                gfx.Begin();
                gfx.Device.Clear(Color.Black);
                gfx.SpriteBatch.Draw(ApplicationSurface.Target, finalDestinationRect, Color.White);
                gfx.End();
                gfx.PopBLendState();
                gfx.PopSamplerState();
            }
        }
        public T AddSceneComponent<T>(T sceneComponent) where T: SceneComponent
        {
            sceneComponent.Scene = this;
            sceneComponent.Initialize();
            _sceneComponents.Add(sceneComponent);
            return sceneComponent;
        }
        public T GetSceneComponent<T>() where T : class
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
            var entity = _pool.GetEntity();
            return AddEntity(entity);
        }
        public Entity AddEntity(Entity entity)
        {
            entity.Scene = this;
            _entities.Add(entity);
            return entity;
        }
        public void DestroyEntity(Entity entity)
        {
            if (_entities.Contains(entity))
            {
                for(var i = entity.Components.Count - 1; i >= 0 ; i--)
                {
                    RemoveComponent(entity, entity.Components[i]);
                }
                _entities.Remove(entity);
                _recycleEntities.Add(entity);
            }
        }
        public void RemoveComponent(Entity entity, Component component)
        {
            if (entity.Components.Contains(component))
            {
                _tracker.RemoveComponent(component);
                component.CleanUp();
                component.RemovedFromEntity();
                entity.Components.Remove(component);
                _recycleComponents.Add(component);
            }

        }
        public T AddComponent<T>(Entity entity, T component) where T: Component
        {
            component.Entity = entity;
            entity.Components.Add(component);
            component.Initialize();
            component.AddedToEntity();
           _tracker.AddComponent(component);
            return component;
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
