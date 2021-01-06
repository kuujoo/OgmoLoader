namespace kuujoo.Pixel
{
    public class ComponentDestroyVisitor : ISortedListVisitor<Component>
    {
        public void Visit(Component item)
        {
            item.Destroy();
        }
    }

    public class ComponentGraphicsDeviceResetVisitor : ISortedListVisitor<Component>
    {
        public void Visit(Component item)
        {
            item.OnGraphicsDeviceReset();
        }
    }

    public class ComponentRemovedFromEntityVisitor : ISortedListVisitor<Component>
    {
        public void Visit(Component item)
        {
            item.RemovedFromEntity();
        }
    }

    public class ComponentUpdateVisitor : ISortedListVisitor<Component>
    {
        public void Visit(Component item)
        {
            if (item.Enabled)
            {
                item.Update();
            }
        }
    }

    public class ComponentCleanUpVisitor : ISortedListVisitor<Component>
    {
        public void Visit(Component item)
        {
            item.CleanUp();
        }
    }
    public class ComponentRenderVisitor : ISortedListVisitor<Component>
    {
        public Graphics Graphics { get; set; }
        public void Visit(Component item)
        {
            if (item.Enabled)
            {
                item.Render(Graphics);
            }
        }
    }
    public static class ComponentListVisitor
    {
        public static ComponentDestroyVisitor DestroyVisitor = new ComponentDestroyVisitor();
        public static ComponentGraphicsDeviceResetVisitor GraphicsDeviceResetVisitor = new ComponentGraphicsDeviceResetVisitor();
        public static ComponentUpdateVisitor UpdateVisitor = new ComponentUpdateVisitor();
        public static ComponentCleanUpVisitor CleanUpVisitor = new ComponentCleanUpVisitor();
        public static ComponentRenderVisitor RenderVisitor = new ComponentRenderVisitor();
        public static ComponentRemovedFromEntityVisitor RemovedFromEntityVisitor = new ComponentRemovedFromEntityVisitor();
    }
}
