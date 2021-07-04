﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MerchantStoreApi.Entities;

namespace MerchantStoreApi.Repositories
{
    public class InMemoryItemsRepository : IItemsRepository
    {
        private readonly List<Item> _items = new()
        {
            new Item  { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow },
            new Item  { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow },
            new Item  { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 14, CreatedDate = DateTimeOffset.UtcNow }
        };

        public IEnumerable<Item> GetItems()
        {
            return _items;
        }        
        
        public Item GetItem(Guid id)
        {
            return _items.Where(item => item.Id == id).SingleOrDefault();
        }

        public void CreateItem(Item item)
        {
            _items.Add(item);
        }

        public void UpdateItem(Item item)
        {
            var index = _items.FindIndex(existingItem => existingItem.Id == item.Id);
            _items[index] = item;
        }

        public void DeleteItem(Guid id)
        {
            var index = _items.FindIndex(existingItem => existingItem.Id == id);
            _items.RemoveAt(index);
        }
    }
}