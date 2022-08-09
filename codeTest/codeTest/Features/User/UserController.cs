using MediatR;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Application.Features.User.Authenticate;
using RapidPay.Application.Features.User.Create;

namespace RapidPay.Application.Features.User
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SignIn")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SignInCommand))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Authenticate(SignInCommand command) => await _mediator.Send(command);

        [HttpPost("SignUp")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateUserCommand))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateUserCommand command) => await _mediator.Send(command);
    }
}
