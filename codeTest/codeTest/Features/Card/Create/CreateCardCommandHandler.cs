using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidPay.Infrastructure.Database;

namespace RapidPay.Application.Features.Card.Create
{
    public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, IActionResult>
    {
        private readonly ILogger<CreateCardCommandHandler> _logger;
        private readonly RapidPayDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCardCommand> _validator;

        public CreateCardCommandHandler(ILogger<CreateCardCommandHandler> logger, RapidPayDbContext context, IMapper mapper, IValidator<CreateCardCommand> validator)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IActionResult> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validation = await _validator.ValidateAsync(request);

                if (!validation.IsValid)
                    return new BadRequestObjectResult(validation.Errors.First().ErrorMessage);

                if (!string.IsNullOrWhiteSpace(request.CardNumber))
                {
                    var cardAlreadyExist = await _context.Cards
                        .AsNoTracking()
                        .AnyAsync(x => x.Number == request.CardNumber)
                        .ConfigureAwait(false);

                    if (cardAlreadyExist)
                        return new ConflictObjectResult("Card already exists."); /* We should have a generic response object for this kind of errors. */

                    var card = new Domain.Card(request.CardNumber);

                    /* Add new card */
                    await _context
                        .AddAsync(card)
                        .ConfigureAwait(false);

                    /* Persist */
                    await _context
                        .SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);

                    var result = _mapper.Map<CreateCardCommandResponse>(card);

                    return new CreatedAtActionResult("Get", nameof(Domain.Card), routeValues: new { CardNumber = result.CardNumber }, result);
                }
                else
                {
                    return new BadRequestResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
