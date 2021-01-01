using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class EntityList : SortedList<Entity>
    {
        Scene _scene;
        public EntityList(Scene scene)
        {
            _scene = scene;
        }
        public void AddEntity(Entity entity)
        {
            entity.Scene = _scene;
            entity.Initialize();
            Add(entity);
        }
        public override void OnItemRemoved(Entity item)
        {
            base.OnItemRemoved(item);
            item.Destroy();
            item.Scene = null;
        }
    }
}
