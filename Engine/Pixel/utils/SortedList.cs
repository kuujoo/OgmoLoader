using System.Collections.Generic;

namespace kuujoo.Pixel
{
    public interface ISortedListVisitor<T>
    {
        public void Visit(T item);
    }

    public class SortedList<T>
    {
        protected List<T> _items = new List<T>();
        protected List<T> _itemsToAdd = new List<T>();
        List<T> _itemsToRemove = new List<T>();
        public int Count => _items.Count;
        public T this[int idx] => _items[idx];
        bool _sort;
        public void MarkListUnsorted()
        {
            _sort = true;
        }
        public void AcceptVisitor(ISortedListVisitor<T> visitor, bool visit_to_be_added_items)
        {
            for(var i = 0; i < _items.Count; i++)
            {
                visitor.Visit(_items[i]);
            }
            if (visit_to_be_added_items)
            {
                for (var i = 0; i < _itemsToAdd.Count; i++)
                {
                    visitor.Visit(_itemsToAdd[i]);
                }
            }
        }
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
        }
        public void Remove(T item)
        {
            if (!_itemsToRemove.Contains(item) &&  (_items.Contains(item) || _itemsToAdd.Contains(item) ) )
            {
                _itemsToRemove.Add(item);
            }  
        }
        public void Clear()
        {
            _itemsToRemove.Clear();
            _items.Clear();
            _itemsToRemove.Clear();
        }
    } 
}
