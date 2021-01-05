using System;

namespace kuujoo.Pixel
{
    public class Buildable : Attribute
    {
        public string Name { get; private set; }
        public Buildable(string name)
        {
            Name = name;
        }
    }
}
