using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RapidPay.Application.Features.User.Create
{
    public class CreateUserCommand : IRequest<IActionResult>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        public CreateUserCommand()
        {
        }
    }
}
