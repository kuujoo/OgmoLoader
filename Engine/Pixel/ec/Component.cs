using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace kuujoo.Pixel
{
    public class Component
    {
        public bool Enabled => (Entity == null || Entity.Enabled) && _enabled;
        public Entity Entity {get; set;}
        public Scene Scene => Entity.Scene;
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
        }
        public virtual void TransformChanged(Transform transform)
        {

        }
        public virtual void Initialize() {
        }
        public virtual void CleanUp() {
        }
        public virtual void RemovedFromScene()
        {
        }
        public virtual void AddedToScene()
        {
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
