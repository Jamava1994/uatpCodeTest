using MediatR;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Application.Features.User.Authenticate;

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

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SignInCommand))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Authenticate(SignInCommand command) => await _mediator.Send(command);
    }
}
