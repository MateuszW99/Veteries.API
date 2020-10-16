using System.Threading.Tasks;
using Animals.Models.Commands;
using Animals.Models.Results;
using Domain.Entities;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veteries.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Route("animals")]
        public async Task<IActionResult> CreateAnimal([FromBody]Animal animal)
        {
            var command = new CreateAnimalCommand() { Animal = animal };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpGet]
        [Route("animals")]
        public async Task<IActionResult> GetAllAnimals()
        {
            var result = await _mediator.Send(new GetAllAnimalsCommand());

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Animals);
        }

        [HttpGet]
        [Route("animals/{id}")]
        public async Task<IActionResult> GetAnimalById(int id)
        {
            var command = new GetAnimalByIdCommand()
            {
                AnimalId = id
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Animal);
        }

        [HttpDelete]
        [Route("animals/{id}")]
        public async Task<IActionResult> DeleteAnimalById(int id)
        {
            var command = new DeleteAnimalCommand()
            {
                AnimalId = id
            };

            DeleteAnimalResult result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost]
        [Route("animals/{id}")]
        public async Task<IActionResult> UpdateAnimal([FromBody] Animal animal)
        {
            var command = new UpdateAnimalCommand() { Animal = animal };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

    }
}
