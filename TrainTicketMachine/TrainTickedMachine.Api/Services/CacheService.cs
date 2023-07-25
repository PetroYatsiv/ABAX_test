using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services;

public class CacheService : ICacheService
{
    private const string TrainStationsCacheKey = "TrainStations";

    private readonly ILogger<CacheService> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientFactory _httpClientFactory;

    public CacheService(ILogger<CacheService> logger,
        IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _httpClientFactory = httpClientFactory;
    }

    public async Task SetCacheFromApi()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync("https://raw.githubusercontent.com/abax-as/coding-challenge/master/station_codes.json");

        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        var trainStations = JsonConvert.DeserializeObject<List<TrainStation>>(jsonString);

        var stationLokup = trainStations.ToLookup(station => station.StationName);

        var expirationTime = DateTimeOffset.Now.AddMinutes(10);
        _memoryCache.Set(TrainStationsCacheKey, stationLokup, expirationTime);
        
        _logger.LogInformation($"SetCacheFromApi: {trainStations.Count} stations");
    }

    public void SetCache(ILookup<string, TrainStation> trainStations)
    {
        var expirationTime = DateTimeOffset.Now.AddMinutes(30);
        _memoryCache.Set(TrainStationsCacheKey, trainStations, expirationTime);

        _logger.LogInformation($"SetCache: {trainStations.Count} stations");
    }

    public void ClearCache()
    {
        _memoryCache.Remove(TrainStationsCacheKey);
    }
}
