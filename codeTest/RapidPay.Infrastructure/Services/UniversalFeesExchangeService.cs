using RapidPay.Domain.Interfaces;
using UniversalFeesExchange.Sdk.Interfaces;

namespace RapidPay.Infrastructure.Services;

public class UniversalFeesExchangeService : IUniversalFeesExchangeService, IDisposable
{
    private readonly IUniversalFeesExchange _ufe;

    /// <summary>
    /// Constructor.
    /// </summary>
    public UniversalFeesExchangeService(IUniversalFeesExchange ufe)
    {
        _ufe = ufe;
    }

    /// <summary>
    /// Calculates the current fee price based on the current time.
    /// </summary>
    /// <returns>Current fee price</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<decimal> GetCurrentFeePriceAsync()
    {
        try
        {
            return await _ufe.GetCurrentFeePriceAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error when getting current fee price.", ex);
        }
    }

    public void Dispose()
    {
        _ufe.Dispose();
    }
}
