using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class EntityGraphicsDeviceResetVisitor : ISortedListVisitor<Entity>
    {
        public void Visit(Entity item)
        {
            item.OnGraphicsDeviceReset();
        }
    }
    public class EntityUpdateVisitor : ISortedListVisitor<Entity>
    {
        public void Visit(Entity item)
        {
            if (item.Enabled)
            {
                item.Update();
            }
        }
    }

    public class EntityCleanUpVisitor : ISortedListVisitor<Entity>
    {
        public void Visit(Entity item)
        {
            item.CleanUp();
        }
    }

    public class EntityRenderVisitor : ISortedListVisitor<Entity>
    {
        public Camera Camera { get; set; }
        public Graphics Graphics { get; set; }
        public void Visit(Entity item)
        {
            if(item.Enabled && Camera.CanSee(item))
            {
                item.Render(Graphics);
            }
        }
    }

    public static class EntityListVisitor
    {
        public static EntityGraphicsDeviceResetVisitor GraphicsDeviceResetVisitor = new EntityGraphicsDeviceResetVisitor();
        public static EntityUpdateVisitor UpdateVisitor = new EntityUpdateVisitor();
        public static EntityCleanUpVisitor CleanUpVisitor = new EntityCleanUpVisitor();
        public static EntityRenderVisitor RenderVisitor = new EntityRenderVisitor();
    }

    public class EntityList : SortedList<Entity>
    {
        Scene _scene;
        EntityLayer _layer;
        public EntityList(Scene scene, EntityLayer layer)
        {
            _scene = scene;
            _layer = layer;
        }
        public void AddEntity(Entity entity)
        {
            entity.Scene = _scene;
            entity.Layer = _layer;
            entity.Initialize();
            Add(entity);
        }
    }
}
