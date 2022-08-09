using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidPay.Domain.Interfaces;
using RapidPay.Infrastructure.Database;

namespace RapidPay.Application.Features.User.Authenticate
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, IActionResult>
    {
        private readonly RapidPayDbContext _context;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        private readonly ILogger<SignInCommandHandler> _logger;

        public SignInCommandHandler(RapidPayDbContext context, IAuthenticationService authenticationService, IMapper mapper, ILogger<SignInCommandHandler> logger)
        {
            _context = context;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                /* Check whether user exist or not */
                var userExist = await _context.Users
                 .AsNoTracking()
                 .AnyAsync(x => x.Username == request.Username)
                 .ConfigureAwait(false);

                if (!userExist)
                    return new BadRequestObjectResult("Specified user does not exist.");

                /* Do authenticate */
                var bearer = await _authenticationService
                    .SignInAsync(_mapper.Map<Domain.User>(request))
                    .ConfigureAwait(false);

                if (bearer != null)
                    return new OkObjectResult(bearer);
                else
                    return new UnauthorizedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
