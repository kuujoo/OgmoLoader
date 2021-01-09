using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace kuujoo.Pixel
{
    public class Scene : IDisposable
    {
        public bool DebugRender { get; set; }
        public SortedList<Entity> Entities { get; private set; }
        public Physics Physics { get; private set; }
        public Color ClearColor { get; set; }
        public Surface ApplicationSurface { get; set; }
        public RuntimeContentManager Content { get; private set; }
        public bool Paused;
        List<Camera> _cameras = new List<Camera>();
        List<SceneComponent> _sceneComponents = new List<SceneComponent>();
        Rectangle _finalDestinationRect;
        public Scene(int game_width, int game_height)
        {
            ClearColor = Color.Aquamarine;
            Content = new RuntimeContentManager();
            Physics = new Physics();
            ApplicationSurface = new Surface(game_width, game_height);
            Entities = new SortedList<Entity>();
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

            Entities.AcceptVisitor(EntityListVisitor.CleanUpVisitor, true);
        }
        public virtual void Update()
        {
            if (Paused) return;
            for (var i = 0; i < _sceneComponents.Count; i++)
            {
                _sceneComponents[i].Update();
            }
            Entities.UpdateLists();
            Entities.AcceptVisitor(EntityListVisitor.UpdateVisitor, false);

        }
        public void OnGraphicsDeviceReset()
        {
            Entities.AcceptVisitor(EntityListVisitor.GraphicsDeviceResetVisitor, true);
            UpdateDrawRect();
        }
        public T FindFirstComponentOfType<T>() where T: Component
        {
            for(var i = 0; i < Entities.Count; i++)
            {
                var c = Entities[i].GetComponent<T>();
                if(c != null)
                {
                    return c;
                }
            }
            return null;
        }
        public void Render()
        {
            // FIXME: sort only when needed
            _cameras.Sort();
            var gfx = Engine.Instance.Graphics;
            for (var i = 0; i < _cameras.Count; i++)
            {
                if (_cameras[i].Enabled)
                {
                    gfx.Begin(_cameras[i]);
                    var visitor = EntityListVisitor.RenderVisitor;
                    visitor.Graphics = gfx;
                    Entities.AcceptVisitor(visitor, false);
                    if(DebugRender)
                    {
                        var debug_visitor = EntityListVisitor.DebugRenderVisitor;
                        debug_visitor.Graphics = gfx;
                        Entities.AcceptVisitor(debug_visitor, false);
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
            entity.Initialize();
            Entities.Add(entity);
            return entity;
        }
        public void DestroyEntity(Entity entity)
        {
            entity.Components.AcceptVisitor(ComponentListVisitor.DestroyVisitor, true);
            entity.Components.AcceptVisitor(ComponentListVisitor.CleanUpVisitor, true);
            entity.Components.AcceptVisitor(ComponentListVisitor.RemovedFromEntityVisitor, true);
            entity.Components.Clear();
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

            _finalDestinationRect.Width = (int)Math.Ceiling((double)surface_w * scale);
            _finalDestinationRect.Height = (int)Math.Ceiling((double)surface_h * scale);
            _finalDestinationRect.X = (sw - _finalDestinationRect.Width) / 2;
            _finalDestinationRect.Y = (sh - _finalDestinationRect.Height) / 2;
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
