using TrainTickedMachine.Api.BackgroundServices;
using TrainTickedMachine.Api.Services;

namespace TrainTickedMachine.Api;

public static class ServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITrainStationFetcher, TrainStationFetcher>();
        services.AddHttpClient();

        services.AddHostedService<TrainStationsCacheService>();

        return services;
    }
}
