using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MerchantStoreApi.Dtos;
using MerchantStoreApi.Entities;
using MerchantStoreApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MerchantStoreApi.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _repository;

        public ItemsController(IItemsRepository repository)
        {
            _repository = repository;
        }

        // GET / items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await _repository.GetItemsAsync()).Select( item => item.AsDto());
            return items;
        }

        // GET / items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await _repository.GetItemAsync(id);

            if (item is null) return NotFound();
            
            return Ok(item.AsDto());
        }

        // POST / items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item newItem = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _repository.CreateItemAsync(newItem);
            // Convention is to return the item you created and a header that specific where you can go ahead and get information about the created item
            return CreatedAtAction(nameof(GetItemAsync), new {id = newItem.Id}, newItem.AsDto());
        }

        // the convention for a put is to not return anything
        // PUT / items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await _repository.GetItemAsync(id);

            if (existingItem is null) return NotFound();

            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await _repository.UpdateItemAsync(updatedItem);
            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            var existingItem = await _repository.GetItemAsync(id);
            if (existingItem is null) return NotFound();

            await _repository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
