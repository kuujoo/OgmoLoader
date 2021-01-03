using System;
using System.Diagnostics.CodeAnalysis;

namespace kuujoo.Pixel
{
    public abstract class Layer : IComparable<Layer>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public abstract void Render(Camera camera);
        public abstract void Update();
        public abstract void CleanUp();
        public abstract void OnGraphicsDeviceReset();

        public int CompareTo(Layer other)
        {
            return Id.CompareTo(other.Id);
        }
    }
}
