using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidPay.Domain.Interfaces;
using RapidPay.Infrastructure.Database;

namespace RapidPay.Application.Features.Card.Pay
{
    public class MakePaymentCommandHandler : IRequestHandler<MakePaymentCommand, IActionResult>
    {
        private readonly RapidPayDbContext _context;
        private readonly IUniversalFeesExchangeService _feeService;
        private readonly ILogger<MakePaymentCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<MakePaymentCommand> _validator;

        public MakePaymentCommandHandler(RapidPayDbContext context, IUniversalFeesExchangeService feeService, ILogger<MakePaymentCommandHandler> logger, IMapper mapper, IValidator<MakePaymentCommand> validator)
        {
            _context = context;
            _feeService = feeService;
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IActionResult> Handle(MakePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validation = await _validator.ValidateAsync(request);

                if (!validation.IsValid)
                    return new BadRequestObjectResult(validation.Errors.First().ErrorMessage);

                using var transaction = await _context.Database
                    .BeginTransactionAsync()
                    .ConfigureAwait(false);

                // Get card by card number.
                var card = await _context.Cards
                    .FirstOrDefaultAsync(x => x.Number == request.CardNumber)
                    .ConfigureAwait(false);

                // Calculate payment total amount using the UFE Service.
                var currentFee = await _feeService
                    .GetCurrentFeePriceAsync()
                    .ConfigureAwait(false);

                var totalAmount = request.PaymentAmount + currentFee;

                // Create a new payment.
                var payment = new Domain.Payment()
                {
                    Card = card,
                    Amount = request.PaymentAmount,
                    FeeApplied = currentFee,
                    Total = totalAmount
                };

                // Update card balance, so by doing this we're avoiding to recalculate on every request.
                card.Balance += totalAmount;

                await _context.Transactions
                    .AddAsync(payment)
                    .ConfigureAwait(false);

                await _context.SaveChangesAsync()
                    .ConfigureAwait(false);

                await transaction.CommitAsync()
                    .ConfigureAwait(false);

                var response = _mapper.Map<MakePaymentCommandResponse>(payment);

                return new CreatedAtActionResult("Get", nameof(Domain.Card), routeValues: new { CardNumber = card.Number }, card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
