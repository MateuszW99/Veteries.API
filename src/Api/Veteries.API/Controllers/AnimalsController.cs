using System.Threading.Tasks;
using Animals.Models.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veteries.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnimalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnimalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAnimalCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success ? (IActionResult)Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllAnimalsCommand());

            return Ok(result.Animals);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetAnimalByIdCommand{AnimalId = id});

            return result.Success ? (IActionResult)Ok(result.Animal) : BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await _mediator.Send(new DeleteAnimalCommand{ AnimalId = id });

            return result.Success ? (IActionResult) Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Update([FromBody]UpdateAnimalCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success ? (IActionResult)Ok(result.Message) : BadRequest(result.Message);
        }

    }
}
