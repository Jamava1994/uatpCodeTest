namespace UniversalFeesExchange.Sdk.Interfaces
{
    public interface IUniversalFeesExchange : IDisposable
    {
        Task<decimal> GetCurrentFeePriceAsync();
    }
}
