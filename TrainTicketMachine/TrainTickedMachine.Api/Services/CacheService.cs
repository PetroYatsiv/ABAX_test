using Microsoft.Extensions.Caching.Memory;
using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services;

public class CacheService : ICacheService
{
    private readonly ILogger<CacheService> _logger;
    private const string TrainStationsCacheKey = "TrainStations";
    private readonly IMemoryCache _memoryCache;
    private readonly ITrainStationFetcher _trainStationFetcher;
    
    public CacheService(
        ILogger<CacheService> logger, 
        IMemoryCache memoryCache, 
        ITrainStationFetcher trainStationFetcher)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _trainStationFetcher = trainStationFetcher;
    }
    public async Task<ILookup<string, TrainStation>>? SetCache()
    {
        var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(12))
                .SetAbsoluteExpiration(TimeSpan.FromDays(1));

        if (_memoryCache.TryGetValue(TrainStationsCacheKey, out ILookup<string, TrainStation> trainStations))
            return trainStations;

        var freshCachedStations = await _trainStationFetcher.GetStationsFromApiAsync();

        _logger.LogInformation("Updating cache with fresh data");
        _memoryCache.Set(TrainStationsCacheKey, freshCachedStations, cacheOptions);

        return freshCachedStations;
    }

    public void SetCache(ILookup<string, TrainStation> trainStations)
    {
        var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(12))
                .SetAbsoluteExpiration(TimeSpan.FromDays(1));

        _memoryCache.Set(TrainStationsCacheKey, trainStations, cacheOptions);
    }

    public void ClearCache()
    {
        _memoryCache.Remove(TrainStationsCacheKey);
    }
}
