using Microsoft.Extensions.DependencyInjection;
using UniversalFeesExchange.Sdk.Interfaces;

namespace UniversalFeesExchange.Sdk
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUFE(this IServiceCollection services) => services.AddSingleton(ufeFactoryMethod);

        private static IUniversalFeesExchange ufeFactoryMethod(IServiceProvider arg) => Exchange.GetInstance();
    }
}
