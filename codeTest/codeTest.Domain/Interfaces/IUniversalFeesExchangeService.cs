namespace RapidPay.Domain.Interfaces
{
    /// <summary>
    /// Universal Fees Exchange Service abstraction, aka UFE.
    /// UFE is a service used to get the current fee price, it could vary depending on the time.
    /// </summary>
    public interface IUniversalFeesExchangeService
    {
        Task<decimal> GetCurrentFeePriceAsync();
    }
}
