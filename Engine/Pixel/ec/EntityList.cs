using System.Collections.Generic;

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
        public List<T> FindComponents<T>() where T : Component
        {
            var list = ListPooler<T>.Obtain();
            for (var i = 0; i < _items.Count; i++)
            {
                var c = _items[i].GetComponent<T>();
                if (c != null)
                {
                    list.Add(c);
                }
            }
            for (var i = 0; i < _itemsToAdd.Count; i++)
            {
                var c = _itemsToAdd[i].GetComponent<T>();
                if (c != null)
                {
                    list.Add(c);
                }
            }
            return list;
        }
    }
}
