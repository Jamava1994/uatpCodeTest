namespace RapidPay.Application.Features.Card.Create
{
    public class CreateCardCommandResponse
    {
        public string? CardNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
