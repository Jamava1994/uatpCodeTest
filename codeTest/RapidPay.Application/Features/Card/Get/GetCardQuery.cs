using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RapidPay.Application.Features.Card.Get
{
    public class GetCardQuery : IRequest<IActionResult>
    {
        [FromRoute]
        public string? CardNumber { get; set; }
    }
}
