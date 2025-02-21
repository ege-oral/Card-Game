using System;
using System.Collections.Generic;

namespace Common
{
    public class ReactiveList<T>
    {
        private readonly List<T> _items = new();

        public event Action<T> ItemAdded;
        public event Action<List<T>> ItemsAdded;
        public event Action<T> ItemRemoved;
        public event Action ListCleared;
        public event Action<IReadOnlyList<T>> ListReplaced;
        public event Action<T, int> ItemInserted;

        public IReadOnlyList<T> Items => _items;

        public void Add(T item)
        {
            _items.Add(item);
            ItemAdded?.Invoke(item);
        }

        public void AddRange(List<T> items)
        {
            _items.AddRange(items);
            ItemsAdded?.Invoke(items);
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > _items.Count) return;
            
            _items.Insert(index, item);
            ItemInserted?.Invoke(item, index);
        }

        public void Replace(List<T> newList)
        {
            if (newList == null) return;
            
            _items.Clear();
            _items.AddRange(newList);
            ListReplaced?.Invoke(_items);
        }

        public void Remove(T item)
        {
            if (_items.Remove(item))
                ItemRemoved?.Invoke(item);
        }

        public void Clear()
        {
            if (_items.Count == 0) return;
            _items.Clear();
            ListCleared?.Invoke();
        }

        public int Count => _items.Count;

        public T this[int index] => _items[index];
    }
}