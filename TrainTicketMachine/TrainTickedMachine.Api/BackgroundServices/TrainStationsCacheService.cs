using Microsoft.Extensions.Caching.Memory;
using TrainTickedMachine.Api.Services;
using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.BackgroundServices;

//background worker, for periodically updating the cache,
//by fetching data from the central system and storing it in the cache
public class TrainStationsCacheService : BackgroundService
{
    private readonly ICacheService _cacheService;

    public TrainStationsCacheService(
        ICacheService cacheService)
    {
        _cacheService = cacheService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _cacheService.SetCache();
        }
    }
}
