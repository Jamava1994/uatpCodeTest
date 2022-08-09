using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Application.Features.User.Authenticate;
using RapidPay.Domain.Interfaces;
using RapidPay.Infrastructure.Database;

namespace RapidPay.Application.Features.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IActionResult>
    {
        private readonly RapidPayDbContext _context;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        private readonly ILogger<SignInCommandHandler> _logger;

        public CreateUserCommandHandler(RapidPayDbContext context, IAuthenticationService authenticationService, IMapper mapper, ILogger<SignInCommandHandler> logger)
        {
            _context = context;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<IActionResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
