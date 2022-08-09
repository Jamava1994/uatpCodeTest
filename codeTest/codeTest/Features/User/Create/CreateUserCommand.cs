using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RapidPay.Application.Features.User.Create
{
    public class CreateUserCommand : IRequest<IActionResult>
    {
        public CreateUserCommand()
        {

        }
    }
}
