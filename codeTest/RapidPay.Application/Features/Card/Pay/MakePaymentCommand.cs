using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RapidPay.Application.Features.Card.Pay
{
    public class MakePaymentCommand : IRequest<IActionResult>
    {
        [FromRoute]
        public string? CardNumber { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
