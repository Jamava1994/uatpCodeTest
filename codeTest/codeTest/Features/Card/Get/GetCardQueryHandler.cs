using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidPay.Infrastructure.Database;

namespace RapidPay.Application.Features.Card.Get
{
    public class GetCardQueryHandler : IRequestHandler<GetCardQuery, IActionResult>
    {
        private readonly RapidPayDbContext _context;
        private readonly ILogger<GetCardQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetCardQueryHandler(RapidPayDbContext dbContext, ILogger<GetCardQueryHandler> logger, IMapper mapper)
        {
            _context = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetCardQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var data = new GetCardQueryResponse();

                var card = await _context.Cards
                        .AsNoTracking()
                        .Where(c => c.Number == request.CardNumber)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                

                if (card == null)
                    return new NotFoundResult();

                return new OkObjectResult(_mapper.Map<GetCardQueryResponse>(card));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }
    }
}
