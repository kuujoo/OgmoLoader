﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public interface IRenderable
    {
        bool Enabled { get; }
        int Layer { get; }
        bool IsVisibleFromCamera(Camera camera);
        void Render(Graphics graphics);
        void DebugRender(Graphics graphics);
    }
    public interface IUpdateable
    {
        bool Enabled { get; }
        int UpdateOrder { get; }
        void Update();
    }
    public class Tracker : SceneComponent
    {
        SpatialHash _hash = new SpatialHash();

        List<IRenderable> _renderables = new List<IRenderable>();
        List<IUpdateable> _updateables = new List<IUpdateable>();
        List<IResettable> _resettables = new List<IResettable>();
        Dictionary<Type, List<Component>> _components = new Dictionary<Type, List<Component>>();

        public void AddComponent(Component component)
        {
            var type = component.GetType();
            if(component is IRenderable)
            {
                _renderables.Add(component as IRenderable);
            }
            if(component is IUpdateable)
            {
                _updateables.Add(component as IUpdateable);
            }

            if(component is IResettable)
            {
                _resettables.Add(component as IResettable);
            }

            if(component is Collider)
            {
                var collider = component as Collider;
                collider.Updated += UpdateCollider;
                _hash.Register(collider);
            }

            List<Component> components;
            if(_components.TryGetValue(type, out components))
            {
                components.Add(component);
            } 
            else
            {
                components = new List<Component>();
                components.Add(component);
                _components[type] = components;
            }
        }
        public void RemoveComponent(Component component)
        {
            var type = component.GetType();
            if (component is IRenderable)
            {
                _renderables.Remove(component as IRenderable);
            }
            if(component is IUpdateable)
            {
                _updateables.Remove(component as IUpdateable);
            }
            if (component is IResettable)
            {
                _resettables.Remove(component as IResettable);
            }
            if (component is Collider)
            {
                var collider = component as Collider;
                _hash.Unregister(collider);
                collider.Updated -= UpdateCollider;
            }
            _components[type].Remove(component);
        }
        public List<IRenderable> GetRenderables()
        {
            _renderables.Sort(RenderableSorter);
            return _renderables;
        }
        public List<IUpdateable> GetUpdateables()
        {
            _updateables.Sort(UpdateableSorter);
            return _updateables;
        }
        public List<IResettable> GetResettables()
        {
            return _resettables;
        }
        public void UpdateCollider(Collider collider)
        {
            _hash.Unregister(collider);
            _hash.Register(collider);
        }
        public T FindComponent<T>() where T : Component
        {
            var type = typeof(T);
            if(_components.ContainsKey(type) == true)
            {
                return _components[type][0] as T;
            }
            else
            {
                return null;
            }
        }
        public void GetComponents<T>(ref List<T> components) where T : Component
        {
            var type = typeof(T);
            List<Component> cs = null;
            if (_components.TryGetValue(type, out cs))
            {
                for (var i = 0; i < cs.Count; i++)
                {
                    components.Add(cs[i] as T);
                }
            }
        }
        public List<Collider> Check(ref Rectangle rect, int mask)
        {
            return _hash.Check(ref rect, mask);
        }
        public List<Collider> Near(ref Rectangle rect, int mask)
        {
            return _hash.Near(ref rect, mask);
        }
        int RenderableSorter(IRenderable r0, IRenderable r1)
        {
            return r0.Layer.CompareTo(r1.Layer);
        }
        int UpdateableSorter(IUpdateable u0, IUpdateable u1)
        {
            return u0.UpdateOrder.CompareTo(u1.UpdateOrder);
        }
    }
}
