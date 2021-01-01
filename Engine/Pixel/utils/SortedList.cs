using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class SortedList<T>
    {
        List<T> _items = new List<T>();
        List<T> _itemsToAdd = new List<T>();
        List<T> _itemsToRemove = new List<T>();
        public int Count => _items.Count;
        public T this[int idx] => _items[idx];
        bool _sort;
        public void MarkListUnsorted()
        {
            _sort = true;
        }
        public void UpdateLists()
        {
            if(_itemsToRemove.Count > 0)
            {
                for(var i = 0; i < _itemsToRemove.Count; i++)
                {
                    var item = _itemsToRemove[i];
                    if (_items.Contains(item))
                    {
                        _items.Remove(item);
                    } else if(_itemsToAdd.Contains(item))
                    {
                        _itemsToAdd.Remove(item);
                    }
                }

            }
            if(_itemsToAdd.Count > 0)
            {
                for (var i = 0; i < _itemsToAdd.Count; i++)
                {
                    _items.Add(_itemsToAdd[i]);
                }
                _itemsToAdd.Clear();
                MarkListUnsorted();
            }
            if(_sort)
            {
                _items.Sort();
                _sort = false;
            }
        }
        public void Add(T item)
        {
            _itemsToAdd.Add(item);
            OnItemAdded(item);
        }
        public void Remove(T item)
        {
            if (!_itemsToRemove.Contains(item) &&  (_items.Contains(item) || _itemsToAdd.Contains(item) ) )
            {
                _itemsToRemove.Add(item);
                OnItemRemoved(item);
            }  
        }
        public void RemoveAll()
        {
            for(var i = 0; i < _items.Count; i++)
            {
                Remove(_items[i]);
            }
            for (var i = 0; i < _itemsToAdd.Count; i++)
            {
                Remove(_itemsToAdd[i]);
            }
        }
        public virtual void OnItemRemoved(T item)
        {

        }
        public virtual void OnItemAdded(T item) { }
    } 
}
