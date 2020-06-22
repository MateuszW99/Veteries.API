using System.Threading.Tasks;
using Animal.Models.Commands;
using Animal.Models.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
                return BadRequest(result);
            }

            return Ok(result.Pet);
        }

        [HttpGet]
        [Route("animals")]
        public async Task<IActionResult> Animals()
        {
            var result = await _mediator.Send(new GetAllAnimalsCommand());
            return Ok(result.Pets);
        }

        [HttpGet]
        [Route("animal/{id}")]
        public async Task<IActionResult> Animal(int id)
        {
            var command = new GetAnimalCommand() {Id = id};

            GetAnimalResult result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest();
            }

            return Ok(result.Pet);
        }
    }
}
