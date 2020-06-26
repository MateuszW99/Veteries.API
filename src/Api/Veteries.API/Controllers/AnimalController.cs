using System.Threading.Tasks;
using Animal.Models.Commands;
using Animal.Models.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Veteries.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnimalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("animal")]
        public async Task<IActionResult> Animal([FromBody] CreateAnimalCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Pet);
        }

        [HttpGet]
        [Route("animals")]
        public async Task<IActionResult> GetAllAnimals()
        {
            var result = await _mediator.Send(new GetAllAnimalsCommand());
            return Ok(result.Pets);
        }

        [HttpGet]
        [Route("animal/{id}")]
        public async Task<IActionResult> GetAnimalById(int id)
        {
            var command = new GetAnimalByIdCommand() {Id = id};

            GetAnimalResult result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Pet);
        }

        [HttpDelete]
        [Route("animal/{id}")]
        public async Task<IActionResult> DeleteAnimalById(int id)
        {
            var command = new DeleteAnimalCommand() { Id = id };

            DeleteAnimalResult result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost]
        [Route("animal/{id}/{name}/{age}/{species}")]
        public async Task<IActionResult> UpdateAnimal(int id, string name, int? age, string species)
        {
            var command = new UpdateAnimalCommand() { Id = id, Name = name, Age = age.Value, Species = species };

            UpdateAnimalResult result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Pet);

        }
    }
}
