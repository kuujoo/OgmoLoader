using System;

namespace kuujoo.Pixel
{
    public class ReflectionEntityBuilder : IEntityBuilder
    {
        string _typePrefix;
        string _createMethod;
        Scene _scene;
        public ReflectionEntityBuilder(Scene scene, string typeprefix, string createmethod)
        {
            _typePrefix = typeprefix;
            _createMethod = createmethod;
        }

        public Entity BuildEntity(string name)
        {
            name = _typePrefix + name;
            var type = Type.GetType(name);
            var creator = type.GetMethod(_createMethod);
            return creator.Invoke(null, new Object[] { _scene }) as Entity;
        }
    }
}
