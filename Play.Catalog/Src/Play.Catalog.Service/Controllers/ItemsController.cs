using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsRepository itemsRepository = new();

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            var item = new Item { Name = createItemDto.Name, Description = createItemDto.Description, Price = createItemDto.Price, CreatedDate = DateTimeOffset.UtcNow };
            await itemsRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var exitingItem = await itemsRepository.GetAsync(id);
            if (exitingItem == null)
            {
                return NotFound();
            }

            exitingItem.Name = updateItemDto.Name;
            exitingItem.Description = updateItemDto.Description;
            exitingItem.Price = updateItemDto.Price;
            await itemsRepository.UpdateAsync(exitingItem);

            return NoContent();
        }

        //Delete /items/{id}
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            // var index = items.FindIndex(e => e.Id == id);
            // items.RemoveAt(index);
            var exitingItem = await itemsRepository.GetAsync(id);
            if (exitingItem == null)
            {
                return NotFound();
            }

            await itemsRepository.RemoveAsync(exitingItem.Id);

            return NoContent();
        }

    }
}