namespace kuujoo.Pixel
{
    public class EntityList : SortedList<Entity>
    {
        public T FindComponent<T>() where T : Component
        {
            for (var i = 0; i < _items.Count; i++)
            {
                var c = _items[i].GetComponent<T>();
                if (c != null)
                {
                    return c;
                }
            }
            for (var i = 0; i < _itemsToAdd.Count; i++)
            {
                var c = _itemsToAdd[i].GetComponent<T>();
                if (c != null)
                {
                    return c;
                }
            }
            return null;
        }
        
        public void Update()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].Components.UpdateLists();
            }
            for (var i = 0; i < _items.Count; i++)
            {           
                _items[i].Components.Update();
            }
        }
        public void CleanUp()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].Components.CleanUp();
            }
        }
        public void OnGraphicsDeviceReset()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].Components.OnGraphicsDeviceReset();
            }
        }
        public void Render(Graphics gfx)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                    _items[i].Components.Render(gfx);
            }
        }
        public void DebugRender(Graphics gfx)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].Components.DebugRender(gfx);
            }
        }
    }
}
