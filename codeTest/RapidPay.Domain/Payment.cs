namespace RapidPay.Domain
{
    /// <summary>
    /// Payment entity.
    /// </summary>
    public class Payment
    {
        public long Id { get; set; }
        public Card? Card { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public decimal FeeApplied { get; set; }

        public Payment()
        {

        }
    }
}
