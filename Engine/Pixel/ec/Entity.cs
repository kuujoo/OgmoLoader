using Microsoft.Xna.Framework;
using System;
using System.Collections;
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
            for(var i = 0; i < Components.Count; i++)
            {
                if(Components[i] is T)
                {
                    return Components[i] as T;
                }
            }
            return null;
        }
        public void SetDepth(int depth)
        {
            if(Depth != depth)
            {
                Depth = depth;
                Scene.Entities.MarkListUnsorted();
            }
        }
        public virtual void Render(Graphics graphics)
        {
            for (var i = 0; i < Components.Count; i++)
            {
                if (Components[i].Enabled)
                {
                    Components[i].Render(graphics);
                }
            }
        }
        public virtual void OnGraphicsDeviceReset()
        {
            for(var i = 0; i < Components.Count; i++)
            {
                Components[i].OnGraphicsDeviceReset();
            }
        }
        public virtual void Destroy()
        {
            Components.RemoveAll();
        }
        public virtual void Update()
        {
            Components.UpdateLists();
            for (var i = 0; i < Components.Count; i++)
            {
                if (Components[i].Enabled)
                {
                    Components[i].Update();
                }
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
