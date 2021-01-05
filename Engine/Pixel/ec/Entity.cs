using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace kuujoo.Pixel
{
    public class Entity : IComparable<Entity>
    {
        public Vector2 Position;
        public bool Enabled => _enabled;
        public uint Id { get; private set; }
        public int Depth { get; set; }
        public string Name { get; set; }
        public int Tag { get; set; }
        public SortedList<Component> Components { get; private set; }
        public EntityLayer Layer { get; set; }
        public Scene Scene => Layer.Scene;

        static uint _idGenerator = 0;
        bool _enabled = true;
        public Entity()
        {
            Components = new SortedList<Component>();
            Id = _idGenerator;
            _idGenerator++;
        }
        public virtual void Initialize() { }
        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
        }
        public Component AddComponent(Component component)
        {
            component.Entity = this;
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
                Layer.Entities.MarkListUnsorted();
            }
        }
        public int CompareTo(Entity other)
        {
            var compare = Depth.CompareTo(other.Depth);
            if(compare == 0)
            {
                compare = Id.CompareTo(other.Id);
            }
            return compare;
        }
        public ICoroutine StartCoroutine(IEnumerator enumerator)
        {
            return Engine.Instance.StartCoroutine(enumerator);
        }
    }
}
