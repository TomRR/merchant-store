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
        public IEnumerable<ItemDto> GetItems()
        {
            var items = _repository.GetItems().Select( item => item.AsDto());
            return items;
        }

        // GET / items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = _repository.GetItem(id);

            if (item is null) return NotFound();
            
            return Ok(item.AsDto());
        }

        // POST / items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item newItem = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            _repository.CreateItem(newItem);
            // Convention is to return the item you created and a header that specific where you can go ahead and get information about the created item
            return CreatedAtAction(nameof(GetItem), new {id = newItem.Id}, newItem.AsDto());
        }

        // the convention for a put is to not return anything
        // PUT / items/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = _repository.GetItem(id);

            if (existingItem is null) return NotFound();

            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            _repository.UpdateItem(updatedItem);
            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = _repository.GetItem(id);
            if (existingItem is null) return NotFound();

            _repository.DeleteItem(id);
            return NoContent();
        }
    }
}
