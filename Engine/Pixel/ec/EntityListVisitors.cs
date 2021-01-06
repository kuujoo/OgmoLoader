using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class EntityGraphicsDeviceResetVisitor : ISortedListVisitor<Entity>
    {
        public void Visit(Entity item)
        {
            item.Components.AcceptVisitor(ComponentListVisitor.GraphicsDeviceResetVisitor, true);
        }
    }
    public class EntityUpdateVisitor : ISortedListVisitor<Entity>
    {
        public void Visit(Entity item)
        {
            if (item.Enabled)
            {
                item.Components.UpdateLists();
                item.Components.AcceptVisitor(ComponentListVisitor.UpdateVisitor, false);
            }
        }
    }

    public class EntityCleanUpVisitor : ISortedListVisitor<Entity>
    {
        public void Visit(Entity item)
        {
            item.Components.AcceptVisitor(ComponentListVisitor.CleanUpVisitor, false);
            item.Components.Clear();
        }
    }

    public class EntityRenderVisitor : ISortedListVisitor<Entity>
    {
        public Graphics Graphics { get; set; }
        public void Visit(Entity item)
        {
            if(item.Enabled && Graphics.Camera.CanSee(item))
            {
                var visitor = ComponentListVisitor.RenderVisitor;
                visitor.Graphics = Graphics;
                item.Components.AcceptVisitor(visitor, false);
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

}
