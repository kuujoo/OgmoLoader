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
        public Entity BuildEntity(string buildableclass)
        {
            var b = _buildables[buildableclass];
            return Activator.CreateInstance(b) as Entity;
        }
        public FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            FieldInfo fieldInfo = null;
            do
            {
                fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            } while (fieldInfo == null && type != null);
            return fieldInfo;
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
                    if (buildable != null && typeof(Entity).IsAssignableFrom(type))
                    {
                        _buildables[type.Name] = type;
                    }
                }
            }
        }
    }
}
