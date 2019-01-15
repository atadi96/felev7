using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zh.Persistence;
using Zh.Persistence.DTO;

namespace Zh.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ZhContext context;

        public ValuesController(ZhContext context)
        {
            this.context = context;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<ItemPreviewDTO> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ItemDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] ItemDTO value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
