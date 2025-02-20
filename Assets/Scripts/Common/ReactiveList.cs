using System;
using System.Collections.Generic;

namespace Common
{
    public class ReactiveList<T>
    {
        private readonly List<T> _items = new();
        public event Action<List<T>> ListChanged;

        public IReadOnlyList<T> Items => _items;

        public void Add(T item)
        {
            _items.Add(item);
            ListChanged?.Invoke(_items);
        }

        public void AddRange(IEnumerable<T> items)
        {
            _items.AddRange(items);
            ListChanged?.Invoke(_items);
        }

        public void Remove(T item)
        {
            if (_items.Remove(item))
                ListChanged?.Invoke(_items);
        }

        public void Clear()
        {
            if (_items.Count == 0) return;
            _items.Clear();
            ListChanged?.Invoke(_items);
        }

        public int Count => _items.Count;

        public T this[int index] => _items[index]; // Indexer for easy access
    }
}