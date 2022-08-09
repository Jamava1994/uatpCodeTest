using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IValidator<CreateUserCommand> _validator;

        public CreateUserCommandHandler(RapidPayDbContext context, IAuthenticationService authenticationService, IMapper mapper, ILogger<SignInCommandHandler> logger, IValidator<CreateUserCommand> validator)
        {
            _context = context;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<IActionResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validation = await _validator.ValidateAsync(request);

                if (!validation.IsValid)
                    return new BadRequestObjectResult(validation.Errors.First().ErrorMessage);

                var userAlreadyExist = await _context.Users
                  .AsNoTracking()
                  .AnyAsync(x => x.Username == request.Username)
                  .ConfigureAwait(false);

                if (userAlreadyExist)
                    return new ConflictObjectResult("User already exists.");

                /* Calculate MD5 Hash */
                request.Password = await _authenticationService
                    .CalculateHashAsync(request.Password)
                    .ConfigureAwait(false);

                var user = _mapper.Map<Domain.User>(request);

                await _context.AddAsync(user)
                    .ConfigureAwait(false);

                await _context.SaveChangesAsync()
                    .ConfigureAwait(false);

                return new CreatedResult(String.Empty, user); /* Get users endpoint not implemented */
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
