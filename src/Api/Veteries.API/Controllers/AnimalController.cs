using System.Threading.Tasks;
using Animals.Models.Commands;
using Animals.Models.Results;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veteries.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnimalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnimalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] CreateAnimalCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success ? (IActionResult)Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimals()
        {
            var result = await _mediator.Send(new GetAllAnimalsCommand());

            return result.Success ? (IActionResult)Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimalById([FromBody] GetAnimalByIdCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success ? (IActionResult)Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimalById([FromBody]DeleteAnimalCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success ? (IActionResult) Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateAnimal([FromBody]UpdateAnimalCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Success ? (IActionResult)Ok(result.Message) : BadRequest(result.Message);
        }

    }
}
