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
        public Scene Scene { get; set; }
        public ComponentList Components { get; private set; }
        public EntityLayer Layer { get; set; }

        static uint _idGenerator = 0;
        bool _enabled = true;
        public Entity()
        {
            Components = new ComponentList(this);
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
            Components.AddComponent(component);
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
        public virtual void Render(Graphics graphics)
        {
            var visitor = ComponentListVisitor.RenderVisitor;
            visitor.Graphics = graphics;
            Components.AcceptVisitor(visitor, false);
        }
        public virtual void OnGraphicsDeviceReset()
        {
            Components.AcceptVisitor(ComponentListVisitor.GraphicsDeviceResetVisitor, true);
        }
        public virtual void Destroy()
        {
            Components.AcceptVisitor(ComponentListVisitor.DestroyVisitor, true);
        }
        public virtual void CleanUp()
        {
            Components.AcceptVisitor(ComponentListVisitor.CleanUpVisitor, true);
            Components.Clear();
        }
        public virtual void Update()
        {
            Components.UpdateLists();
            Components.AcceptVisitor(ComponentListVisitor.UpdateVisitor, false);
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
