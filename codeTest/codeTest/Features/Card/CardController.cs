using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Application.Features.Card.Create;
using RapidPay.Application.Features.Card.Get;
using RapidPay.Application.Features.Card.Pay;

namespace RapidPay.Application.Features.Card
{
    [Authorize()]
    [ApiController]
    [Route("api/cards")]
    public class CardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{CardNumber}/balance")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCardQueryResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAsync([FromQuery] GetCardQuery query) => await _mediator.Send(query);

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateCardCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(CreateCardCommand command) => await _mediator.Send(command);

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MakePaymentCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Pay(MakePaymentCommand command) => await _mediator.Send(command);


    }
}