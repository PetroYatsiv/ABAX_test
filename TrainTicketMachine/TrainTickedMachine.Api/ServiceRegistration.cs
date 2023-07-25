using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using TrainTickedMachine.Api.BackgroundServices;
using TrainTickedMachine.Api.Services;

namespace TrainTickedMachine.Api;

public static class ServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHostedService<BackgroundCacheCreator>();

        services.AddScoped<ITrainStationFetcher, TrainStationFetcher>();
        services.AddScoped<ITrainStationSearcher, TrainStationSearcher>();
        services.AddHttpClient();

        services.AddScoped<ICacheService, CacheService>();
        services.AddMemoryCache();
        services.AddSingleton<CacheService>();
        return services;
    }
}
