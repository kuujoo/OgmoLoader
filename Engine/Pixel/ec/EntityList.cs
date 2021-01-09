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
    }
}
