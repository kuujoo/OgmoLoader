using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace kuujoo.Pixel
{
    public class Component : IComparable<Component>
    {
        public bool Enabled => (Entity == null || Entity.Enabled) && _enabled;
        public Entity Entity {get; set;}
        public Scene Scene => Entity.Scene;
        public int UpdateOrder { get; set; }
        bool _enabled = true;
        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
        }
        public void AddedToEntity()
        {
            Entity.Transform.Changed += TransformChanged;
            Scene.Tracker.AddComponent(this);
        }
        public void RemovedFromEntity()
        {
            Entity.Transform.Changed -= TransformChanged;
            Scene.Tracker.RemoveComponent(this);
        }
        public virtual void TransformChanged(Transform transform)
        {

        }
        public virtual void Initialize() {
        }
        public virtual void Destroy() { }
        public virtual void CleanUp() {
        }
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
