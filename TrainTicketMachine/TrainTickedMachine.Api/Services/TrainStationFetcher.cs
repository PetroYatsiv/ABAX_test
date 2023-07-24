using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services;

public class TrainStationFetcher : ITrainStationFetcher
{
    private readonly ILogger<TrainStationFetcher> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICacheService _cache;

    public TrainStationFetcher(
        ILogger<TrainStationFetcher> logger, 
        IHttpClientFactory httpClientFactory,
        ICacheService cache)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _cache = cache;
    }

    public async Task<Dictionary<string, TrainStation>> GetStationsFromApiAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync("https://raw.githubusercontent.com/abax-as/coding-challenge/master/station_codes.json");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        var trainStations = JsonConvert.DeserializeObject<List<TrainStation>>(jsonString);

        var stationDictionary = trainStations.ToDictionary(station => station.StationCode);
        _cache.SetCache(stationDictionary);
        
        return stationDictionary;
    }
}
