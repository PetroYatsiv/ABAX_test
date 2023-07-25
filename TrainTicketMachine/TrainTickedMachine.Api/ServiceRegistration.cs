using Microsoft.Extensions.Caching.Memory;
using TrainTickedMachine.Api.BackgroundServices;
using TrainTickedMachine.Api.Services;

namespace TrainTickedMachine.Api;

public static class ServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        //services.AddHostedService<TrainStationsCacheService>();


        services.AddScoped<ITrainStationFetcher, TrainStationFetcher>();
        services.AddScoped<ITrainStationSearcher, TrainStationSearcher>();
        services.AddScoped<IMemoryCache, MemoryCache>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddHttpClient();



        return services;
    }
}
