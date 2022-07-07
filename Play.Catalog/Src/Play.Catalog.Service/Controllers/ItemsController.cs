using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new(){
            new ItemDto(Guid.NewGuid(), "Potion","Resore a small amount of HP",5,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote","Cures poison",7,DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze sword","Deals a small amount of damage",20,DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ItemDto GetById(Guid id)
        {
            return items.Where(item => item.Id == id).SingleOrDefault();
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            var exitingItem = items.Where(item => item.Id == id).SingleOrDefault();
            var updatedItem = exitingItem with{
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };
            var index = items.FindIndex(exitingItem => exitingItem.Id == id);
            items[index] = updatedItem;
            return NoContent();
        }

        

    }
}