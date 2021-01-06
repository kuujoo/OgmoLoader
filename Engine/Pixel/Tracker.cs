using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class Tracker
    {
        SpatialHash _hash = new SpatialHash();
        public void RegisterComponent(Component component)
        {
            if(component is ColliderComponent)
            {
                var collider = component as ColliderComponent;
                collider.Updated += OnColliderChanged;
                OnColliderChanged(collider);
            }
        }
        void OnColliderChanged(ColliderComponent collider)
        {
            _hash.Unregister(collider);
            _hash.Register(collider);
        }
        public HashSet<ColliderComponent> Check(ref Rectangle rect, int mask)
        {
            return _hash.Check(ref rect, mask);
        }
        public void UnregisterComponent(Component component)
        {
            if (component is ColliderComponent)
            {
                var collider = component as ColliderComponent;
                _hash.Unregister(collider);
                collider.Updated -= OnColliderChanged;
            }
        }
    }
}
