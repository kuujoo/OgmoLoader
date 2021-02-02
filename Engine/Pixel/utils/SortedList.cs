using System;
using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public class SortedList<T>
    {
        protected List<T> _items = new List<T>(20);
        protected List<T> _itemsToAdd = new List<T>(20);
        List<T> _itemsToRemove = new List<T>(20);
        public int Count => _items.Count;
        public T this[int idx] => _items[idx];
        public I GetItemOfType<I>() where I : class
        {
            for(var i = 0; i < _items.Count; i++)
            {
                if(_items[i] is I)
                {
                    return _items[i] as I;
                }
            }

            for (var i = 0; i < _itemsToAdd.Count; i++)
            {
                if (_itemsToAdd[i] is I)
                {
                    return _itemsToAdd[i] as I;
                }
            }
            return null;
        }

        public List<I> GetItemsOfType<I>() where I : class
        {
            var list = ListPooler<I>.Obtain();
            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i] is I)
                {
                    list.Add(_items[i] as I);
                }
            }

            for (var i = 0; i < _itemsToAdd.Count; i++)
            {
                if (_itemsToAdd[i] is I)
                {
                    list.Add(_items[i] as I);
                }
            }
            return list;
        }
        public void UpdateLists()
        {
            UpdateRemoveList();
            if(_itemsToAdd.Count > 0)
            {
                for (var i = 0; i < _itemsToAdd.Count; i++)
                {
                    _items.Add(_itemsToAdd[i]);
                }
                _itemsToAdd.Clear();
            }
        }
        public void UpdateRemoveList()
        {
            if (_itemsToRemove.Count > 0)
            {
                for (var i = 0; i < _itemsToRemove.Count; i++)
                {
                    var item = _itemsToRemove[i];
                    if (_items.Contains(item))
                    {
                        _items.Remove(item);
                    }
                    else if (_itemsToAdd.Contains(item))
                    {
                        _itemsToAdd.Remove(item);
                    }
                }
                _itemsToRemove.Clear();
            }
        }
        public void Add(T item)
        {
            _itemsToAdd.Add(item);
        }
        public void Remove(T item)
        {
            if (!_itemsToRemove.Contains(item) &&  (_items.Contains(item) || _itemsToAdd.Contains(item) ) )
            {
                _itemsToRemove.Add(item);
            }  
        }
       public  bool Contains(T item)
        {
            return _itemsToAdd.Contains(item) || _items.Contains(item);
        }
        public void Clear()
        {
            _itemsToRemove.Clear();
            _items.Clear();
            _itemsToRemove.Clear();
        }
        public void ForEach(Action<T> action)
        {
            for (var i = 0; i < _itemsToAdd.Count; i++)
            {
                action?.Invoke(_itemsToAdd[i]);
            }
            for (var i = 0; i < _items.Count; i++)
            {
                action?.Invoke(_items[i]);
            }
        }
    } 
}
