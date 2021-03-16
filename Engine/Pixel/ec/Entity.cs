using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace kuujoo.Pixel
{
    public class Entity
    {
        public Transform Transform { get; private set; }
        public bool Enabled => _enabled;
        public string Name { get; set; }
        public List<Component> Components { get; private set; }
        public Scene Scene { get; set; }
        bool _enabled = true;
        public Entity()
        {
            Transform = new Transform();
            Components = new List<Component>();
            Transform.Changed = OnTransformChanged;
        }
        void OnTransformChanged(Transform transform)
        {
            for (var i = 0; i < Components.Count; i++)
            {
                Components[i].TransformChanged(transform);
            }
        }
        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
        }
        public T AddComponent<T>(T component) where T: Component
        {
            return Scene.AddComponent(this, component);
        }
        public void RemoveComponent(Component component)
        {
            Scene.RemoveComponent(this, component);
        }
        public T GetComponent<T>() where T : class
        {
            for(var i = 0; i < Components.Count; i++)
            {
                if(Components[i] is T)
                {
                    return Components[i] as T;
                }
            }
            return null;
        }
    }
}
