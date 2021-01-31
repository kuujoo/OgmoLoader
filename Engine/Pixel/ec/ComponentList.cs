namespace kuujoo.Pixel
{
    public class ComponentList : SortedList<Component>
    {
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
            for (var i = 0; i < _itemsToAdd.Count; i++)
            {
                _itemsToAdd[i].CleanUp();
            }
        }
        public void Destroy()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].Destroy();
            }

            for (var i = 0; i < _itemsToAdd.Count; i++)
            {
                _itemsToAdd[i].Destroy();
            }
        }
        public void RemovedFromEntity()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].RemovedFromEntity();
            }
            for (var i = 0; i < _itemsToAdd.Count; i++)
            {
                _itemsToAdd[i].RemovedFromEntity();
            }
        }
        public void Reset()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                var resettable = _items[i] as IResettable;
                if(resettable != null)
                {
                    resettable.Reset();
                }
            }
        }
    }
}
