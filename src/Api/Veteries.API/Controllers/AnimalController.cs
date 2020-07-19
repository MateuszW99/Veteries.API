using System.Linq;
using System.Threading.Tasks;
using Animal.Models.Commands;
using Animal.Models.Results;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account.Manage;
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
        public async Task<IActionResult> CreateAnimal(string name, int age, string species)
        {
            var command = new CreateAnimalCommand()
            {
                Name = name,
                Age = age,
                Species = species,
                UserId = HttpContext.GetUserId()
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Animal);
        }

        [HttpGet]
        [Route("animals")]
        public async Task<IActionResult> GetAllAnimals()
        {
            var result = await _mediator.Send(new GetAllAnimalsCommand());
            return Ok(result.Animals);
        }

        [HttpGet]
        [Route("animals/{id}")]
        public async Task<IActionResult> GetAnimalById(int id)
        {
            var command = new GetAnimalByIdCommand() {Id = id};

            GetAnimalResult result = await _mediator.Send(command);

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
                Id = id,
                UserId = HttpContext.GetUserId()
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
        public async Task<IActionResult> UpdateAnimal(int id, string name, int age, string species)
        {
            var command = new UpdateAnimalCommand()
            {
                Id = id, 
                Name = name, 
                Age = age, 
                Species = species,
                UserId = HttpContext.GetUserId()
            };

            UpdateAnimalResult result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Animal);
        }

    }
}
