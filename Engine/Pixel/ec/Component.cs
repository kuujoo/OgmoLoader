using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace kuujoo.Pixel
{
    public class Component : IComparable<Component>
    {
        public bool Enabled => Entity.Enabled && _enabled;
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
        public virtual void Initialize() { }
        public virtual void Render(Graphics graphics) {}
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
    }
}
