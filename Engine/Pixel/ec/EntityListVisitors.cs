using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class EntityGraphicsDeviceResetVisitor : ISortedListVisitor<Entity>
    {
        public void Visit(Entity item)
        {
            item.OnGraphicsDeviceReset();
            item.Components.AcceptVisitor(ComponentListVisitor.GraphicsDeviceResetVisitor, true);
        }
    }
    public class EntityUpdateVisitor : ISortedListVisitor<Entity>
    {
        public void Visit(Entity item)
        {
            if (item.Enabled)
            {
                item.Update();
                item.Components.UpdateLists();
                item.Components.AcceptVisitor(ComponentListVisitor.UpdateVisitor, false);
            }
        }
    }

    public class EntityCleanUpVisitor : ISortedListVisitor<Entity>
    {
        public void Visit(Entity item)
        {
            item.CleanUp();
            item.Components.AcceptVisitor(ComponentListVisitor.CleanUpVisitor, false);
            item.Components.Clear();
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

}
