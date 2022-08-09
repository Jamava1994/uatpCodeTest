using UniversalFeesExchange.Sdk.Interfaces;
using UniversalFeesExchange.Sdk.Extensions;

namespace UniversalFeesExchange.Sdk
{
    /// <summary>
    /// This is supposed to simulate an external service, instead of calling a remote service. 
    /// So that, meant as an external service. (i.e. PayPal Sdk nuget, etc.).
    /// I've decided to use multi-threading.
    /// </summary>
    public sealed class Exchange : IUniversalFeesExchange
    {
        private const int IntervalLowest = 0;
        private const int IntervalHighest = 2;
        private const int CallbackPeriodHours = 1;

        private decimal? _lastFeePrice;
        private decimal _currentFeePrice;

        private SemaphoreSlim _semaphoreSlim;
        private CancellationTokenSource _cancellationTokenSource;

        private static Exchange? _instance;

        /// <summary>
        /// Singleton implementation.
        /// </summary>
        /// <returns></returns>
        public static Exchange GetInstance()
        {
            if (_instance is null)
                _instance = new Exchange();

            return _instance;
        }

        private Exchange()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _semaphoreSlim = new SemaphoreSlim(0, 1);

            /* 
             * Creates a new thread using Factory. DoCalculateNewFeePrice method will be called once every hour, by doing this we are not
             * going to recalculate the feePrice every time a user makes a request.
             */
            Task.Factory.StartNew(async () =>
            {
                await DoCalculateNewFeePriceAsync(_semaphoreSlim, _cancellationTokenSource.Token);
            });
        }

        private async Task DoCalculateNewFeePriceAsync(SemaphoreSlim semaphore, CancellationToken token = default(CancellationToken))
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var startTime = DateTime.Now;

                    /* Calculate fee price creating a Random object depending on the current day and hour. */
                    var random = new Random(DateTime.Now.Day * DateTime.Now.Hour);

                    var generatedValue = random.NextDecimal(IntervalLowest, IntervalHighest);

                    /* Calculates the new fee price */
                    _currentFeePrice = (_lastFeePrice ?? 1) * generatedValue;

                    _lastFeePrice = _currentFeePrice;

                    var endTime = DateTime.Now;

                    /* This is going to be executed next hour - delta time */
                    var deltaTime = endTime - startTime;

                    _semaphoreSlim.Release();

                    await Task.Delay(TimeSpan.FromHours(CallbackPeriodHours) - deltaTime, token);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Calculate fee price was not possible.", ex);
            }
        }

        public async Task<decimal> GetCurrentFeePriceAsync()
        {
            /* Waits the first call to be done. */
            if (_lastFeePrice is null)
                await _semaphoreSlim.WaitAsync();

            return _currentFeePrice;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _semaphoreSlim.Dispose();
        }
    }
}