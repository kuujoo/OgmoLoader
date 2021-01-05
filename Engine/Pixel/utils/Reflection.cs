using System;
using System.Collections.Generic;
using System.Reflection;

namespace kuujoo.Pixel
{
    public class Reflection
    {
        Dictionary<string, Type> _buildables = new Dictionary<string, Type>();
        public Reflection()
        {
            FindBuildables();
        }
        public Component BuildComponent(string buildableclass)
        {
            var b = _buildables[buildableclass];
            return Activator.CreateInstance(b) as Component;
        }
        private void FindBuildables()
        {
            Type[] types = Assembly.GetEntryAssembly().GetTypes();
            for (var i = 0; i < types.Length; i++)
            {
                var type = types[i];
                object[] customAttributes = type.GetCustomAttributes(typeof(Buildable), false);
                if (customAttributes.Length != 0)
                {
                    var buildable = customAttributes[0] as Buildable;
                    if (buildable != null && typeof(Component).IsAssignableFrom(type))
                    {
                        if(buildable.Name != "")
                        {
                            _buildables[buildable.Name] = type;
                        }
                        else
                        {
                            _buildables[type.Name] = type;
                        }
                       
                    }
                }
            }
        }
    }
}
