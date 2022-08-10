using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RapidPay.Application.Features.Card.Create
{
    public class CreateCardCommand : IRequest<IActionResult>
    {
        public string? CardNumber { get; set; }
    }
}
