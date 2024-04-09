using GraphQLClientApi.GraphQL;
using GraphQLClientApi.OwnerModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraphQLClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OwnerController : ControllerBase
    {
        private readonly OwnerConsumer _consumer;

        public OwnerController(OwnerConsumer consumer)
        {
            _consumer = consumer;
        }

        // GET: OwnerController
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var owners = await _consumer.GetAllOwners();

            return Ok(owners);
        }

        // GET: OwnerController/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var owner = await _consumer.GetOwner(id);
            return Ok(owner);
        }

        // DELETE: OwnerController/Details/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var owner = await _consumer.DeleteOwner(id);
            return Ok(owner);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OwnerInput owner)
        {
            var createdOwner = await _consumer.CreateOwner(owner);
            return Ok(createdOwner);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] OwnerInput owner)
        {
            var updatedOwner = await _consumer.UpdateOwner(id, owner);
            return Ok(updatedOwner);
        }

    }
}
