﻿using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace kuujoo.Pixel
{
    public class Component : IComparable<Component>, IRenderable
    {
        public int Depth => Entity.Depth;
        public bool Enabled => (Entity == null || Entity.Enabled) && _enabled;
        public Entity Entity {get; set;}
        public Scene Scene => Entity.Scene;
        public int UpdateOrder { get; private set; }
        bool _enabled = true;
        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
        }
        public void SetUpdateOrder(int updateOrder)
        {
            if (UpdateOrder != updateOrder)
            {
                UpdateOrder = updateOrder;
                Entity.Components.MarkListUnsorted();
            }
        }
        public void AddedToEntity()
        {
            Entity.Transform.Changed += TransformChanged;
        }
        public void RemovedFromEntity()
        {
            Entity.Transform.Changed -= TransformChanged;
        }
        public virtual void TransformChanged(Transform transform)
        {

        }
        public virtual bool IsVisibleFromCamera(Camera camera)
        {
            return false;
        }
        public virtual void Initialize() { }
        public virtual void Render(Graphics graphics) {}
        public virtual void DebugRender(Graphics graphics) { }
        public virtual void PreUpdate() { }
        public virtual void Update() { }
        public virtual void Destroy() { }
        public virtual void CleanUp() { }
        public virtual void OnGraphicsDeviceReset() { }

        public int CompareTo(Component other)
        {
            return UpdateOrder.CompareTo(other.UpdateOrder);
        }
        public ICoroutine StartCoroutine(IEnumerator enumerator)
        {
            return Engine.Instance.StartCoroutine(enumerator);
        }
        protected T GetSceneComponent<T>() where T: SceneComponent
        {
            return Entity.Scene.GetSceneComponent<T>();
        }
        protected T GetComponent<T>() where T : Component
        {
            return Entity.GetComponent<T>();
        }
    }
}
