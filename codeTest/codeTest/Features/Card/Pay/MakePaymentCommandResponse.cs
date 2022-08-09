namespace RapidPay.Application.Features.Card.Pay
{
    public class MakePaymentCommandResponse
    {
        public Domain.Card? Card { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public decimal FeeApplied { get; set; }
    }
}
