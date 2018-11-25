using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.API.Services;
using Portal.Persistence.DTO;

namespace Portal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Publisher")]
    public class ItemsController : ControllerBase
    {
        private readonly PublishingService service;

        public ItemsController(PublishingService service)
        {
            this.service = service;
        }

        // GET api/items
        [HttpGet]
        public async Task<ItemPreviewDTO[]> Get()
        {
            var previews = await service.GetPreviews();
            return previews;
        }

        // GET api/items/5
        [HttpGet("{id}")]
        public async Task<ItemDataDTO> Get(int id)
        {
            var items = await service.GetItem(id);
            return items;
        }

        // POST api/items
        [HttpPost]
        public async Task<InsertionResultDTO> Post([FromBody] ItemDataDTO itemData)
        {
            var result = await service.InsertItem(itemData);
            return result;
        }

        // PUT api/items/5
        [HttpPut("{id}")]
        public async Task<bool> Put(int id)
        {
            var result = await service.CloseItem(id);
            return result;
        }
    }
}
