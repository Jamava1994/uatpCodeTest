using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RapidPay.Application.Features.User.Authenticate
{
    public class SignInCommand : IRequest<IActionResult>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
