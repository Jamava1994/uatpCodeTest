namespace RapidPay.Domain
{
    /// <summary>
    /// Card entity.
    /// </summary>
    public class Card
    {
        public long Id { get; set; }
        public string Number { get; }
        public decimal Balance { get; set; }

        public Card(string cardNumber)
        {
            Number = cardNumber;
        }

        public Card()
        {

        }
    }
}