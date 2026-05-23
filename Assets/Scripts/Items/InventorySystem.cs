using System;
using System.Collections.Generic;
using UnityEngine;

namespace BananaGame.Items
{
    public class InventorySystem
    {
        private readonly List<BaseItem> _items = new();

        public event Action<BaseItem> OnItemAdded;
        public event Action<BaseItem> OnItemRemoved;

        public void Add(BaseItem item)
        {
            _items.Add(item);
            OnItemAdded?.Invoke(item);
        }

        public void Remove(BaseItem item)
        {
            if (_items.Remove(item))
                OnItemRemoved?.Invoke(item);
        }

        public T GetItem<T>() where T : BaseItem => _items.Find(item => item is T) as T;

        public bool HasItem<T>() where T : BaseItem => _items.Exists(item => item is T);

        public IReadOnlyList<BaseItem> Items => _items;
    }
}
