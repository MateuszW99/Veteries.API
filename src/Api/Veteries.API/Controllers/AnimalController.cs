using System.Threading.Tasks;
using Animal.Models.Commands;
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
        public async Task<IActionResult> Pet([FromBody] AddAnimalCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
