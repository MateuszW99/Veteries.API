using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.Commands;

namespace Veteries.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser.Command command)
        {
            await _mediator.Send(command);
            return Ok();
        }

    }
}