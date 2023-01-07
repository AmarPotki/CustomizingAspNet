using CustomizingAspNet.HostedServices.Caching;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CustomizingAspNet.HostedServices;

public static class CachingServiceCollectionExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();

        services.TryAddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>)); // open generic registration

        services.TryAddSingleton<IDistributedCacheFactory, DistributedCacheFactory>();

        return services;
    }
}