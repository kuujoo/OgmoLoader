using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace kuujoo.Pixel
{

    public class Entity : IComparable<Entity>
    {
        public Transform Transform { get; private set; }
        public bool Enabled => _enabled;
        public int Depth { get; set; }
        public string Name { get; set; }
        public int Tag { get; set; }
        public SortedList<Component> Components { get; private set; }
        public Scene Scene { get; set; }
        bool _enabled = true;
        public Entity()
        {
            Components = new SortedList<Component>();
            Transform = new Transform();
        }
        public virtual void Initialize() { }
        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
        }
        public T AddComponent<T>(T component) where T: Component
        {
            component.Entity = this;
            component.AddedToEntity();
            component.Initialize();
            Components.Add(component);
            return component;
        }
        public T GetComponent<T>() where T : class
        {
            return Components.GetItemOfType<T>();
        }
        public List<T> GetComponents<T>() where T : class
        {
            return Components.GetItemsOfType<T>();
        }
        public void SetDepth(int depth)
        {
            if(Depth != depth)
            {
                Depth = depth;
                Scene.Entities.MarkListUnsorted();
            }
        }
        public int CompareTo(Entity other)
        {
            var compare = Depth.CompareTo(other.Depth);
            return compare;
        }
        public ICoroutine StartCoroutine(IEnumerator enumerator)
        {
            return Engine.Instance.StartCoroutine(enumerator);
        }
    }
}
