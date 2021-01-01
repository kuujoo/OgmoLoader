namespace kuujoo.Pixel
{
    public class ComponentList : SortedList<Component>
    {
        Entity _entity;
        public ComponentList(Entity entity)
        {
            _entity = entity;
        }
        public Component AddComponent(Component component)
        {
            component.Entity = _entity;
            component.Initialize();
            Add(component);
            return component;
        }
        public override void OnItemRemoved(Component item)
        {
            base.OnItemRemoved(item);
            item.Destroy();
            item.Entity = null;
        }
    }
}
