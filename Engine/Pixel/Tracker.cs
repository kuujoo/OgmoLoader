using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class Tracker
    {
        SpatialHash _hash = new SpatialHash();
        public void RegisterComponent(Component component)
        {
            if(component is Collider)
            {
                var collider = component as Collider;
                collider.Updated += OnColliderChanged;
                OnColliderChanged(collider);
            }
        }
        void OnColliderChanged(Collider collider)
        {
            _hash.Unregister(collider);
            _hash.Register(collider);
        }
        public HashSet<Collider> Check(ref Rectangle rect, int mask)
        {
            return _hash.Check(ref rect, mask);
        }
        public void UnregisterComponent(Component component)
        {
            if (component is Collider)
            {
                var collider = component as Collider;
                _hash.Unregister(collider);
                collider.Updated -= OnColliderChanged;
            }
        }
    }
}
