using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace kuujoo.Pixel
{
    public class ComponentPool
    {
        Dictionary<Type, Queue<Component>> _components = new Dictionary<Type, Queue<Component>>();
        public T GetComponent<T>() where T : Component, new()
        {
            var type = typeof(T);
            var componentsQueue = GetQueue(type);
            if (componentsQueue.Count > 0)
            {
                var c = componentsQueue.Dequeue();
                c.SetEnabled(true);
                return c as T;
            }
            else
            {
                return new T();
            }
        }
        Queue<Component> GetQueue(Type type)
        {
            Queue<Component> componentsQueue;
            if (!_components.TryGetValue(type, out componentsQueue))
            {
                componentsQueue = new Queue<Component>();
                _components[type] = componentsQueue;
            }
            return componentsQueue;
        }
        public void Free(Component component)
        {
            var type = component.GetType();
            var queue = GetQueue(type);
            queue.Enqueue(component);
        }
        public void Clear()
        {
            foreach(var v in _components)
            {
                v.Value.Clear();
            }
            _components.Clear();
        }
    }

    public class EntityPool
    {
        Dictionary<Type, Queue<Entity>> _entities = new Dictionary<Type, Queue<Entity>>();
        public T GetComponent<T>() where T : Entity, new()
        {
            var type = typeof(T);
            var componentsQueue = GetQueue(type);
            if (componentsQueue.Count > 0)
            {
                var e = componentsQueue.Dequeue();
                e.SetEnabled(true);
                return e as T;
            }
            else
            {
                return new T();
            }
        }
        Queue<Entity> GetQueue(Type type)
        {
            Queue<Entity> entitiesQueue;
            if (!_entities.TryGetValue(type, out entitiesQueue))
            {
                entitiesQueue = new Queue<Entity>();
                _entities[type] = entitiesQueue;
            }
            return entitiesQueue;
        }
        public void Free(Entity entity)
        {
            var type = entity.GetType();
            var queue = GetQueue(type);
            queue.Enqueue(entity);
        }
        public void Clear()
        {
            foreach (var v in _entities)
            {
                v.Value.Clear();
            }
            _entities.Clear();
        }
    }

    public class ScenePool : SceneComponent
    {
        ComponentPool _componentPool = new ComponentPool();
        EntityPool _entityPool = new EntityPool();
        public override void Initialize()
        {
            base.Initialize();
        }
        public T Get<T>() where T: Component, new()
        {
            return _componentPool.GetComponent<T>();
        }
        public Entity GetEntity()
        {
            return _entityPool.GetComponent<Entity>();
        }
        public void Free(Component component)
        {
            Debug.Assert(component.Entity == null);
            _componentPool.Free(component);
        }
        public void Free(Entity entity)
        {
            Debug.Assert(entity.Components.Count == 0);
            _entityPool.Free(entity);
        }
        public void Clear()
        {
            _componentPool.Clear();
            _entityPool.Clear();
        }
    }
}
