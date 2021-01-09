namespace kuujoo.Pixel
{
    public class ComponentList : SortedList<Component>
    {
        public void Update()
        {
            for(var i = 0; i < _items.Count; i++)
            {
                _items[i].Update();
            }
        }
        public void Render(Graphics gfx)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].Render(gfx);
            }
        }
        public void DebugRender(Graphics gfx)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].DebugRender(gfx);
            }
        }
        public void OnGraphicsDeviceReset()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].OnGraphicsDeviceReset();
            }
        }
        public void CleanUp()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].CleanUp();
            }
        }
        public void Destroy()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].Destroy();
            }
        }
        public void RemovedFromEntity()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].RemovedFromEntity();
            }
        }
    }
}
