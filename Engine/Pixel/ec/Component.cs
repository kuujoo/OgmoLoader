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

        }
        public void RemovedFromEntity()
        {
            Entity.Transform.Changed -= TransformChanged;
            Scene.GetSceneComponent<Tracker>().RemoveComponent(this);
        }
        public virtual void StartUp()
        {

        }
        public virtual void TransformChanged(Transform transform)
        {

        }
        public virtual void Initialize() {
        }
        public virtual void CleanUp() {
        }
        public int CompareTo(Component other)
        {
            return UpdateOrder.CompareTo(other.UpdateOrder);
        }
        public ICoroutine StartCoroutine(IEnumerator enumerator)
        {
            return Engine.Instance.StartCoroutine(enumerator);
        }
        protected T GetSceneComponent<T>() where T: class
        {
            return Entity.Scene.GetSceneComponent<T>();
        }
        public T GetComponent<T>() where T : class
        {
            return Entity.GetComponent<T>();
        }
    }
}
