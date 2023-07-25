using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services;

public class TrainStationFetcher : ITrainStationFetcher
{
    private const string TrainStationsCacheKey = "TrainStations";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;

    public TrainStationFetcher(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
    {
        _httpClientFactory = httpClientFactory;
        _memoryCache = memoryCache;
    }

    public async Task<ILookup<string, TrainStation>> GetStationsFromApiAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync("https://raw.githubusercontent.com/abax-as/coding-challenge/master/station_codes.json");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        var trainStations = JsonConvert.DeserializeObject<List<TrainStation>>(jsonString);

        var stationsLokup = trainStations.ToLookup(station => station.StationName);
        
        return stationsLokup;
    }

    public async Task<ILookup<string, TrainStation>> GetStationsFromCacheAsync()
    {
        if (_memoryCache.TryGetValue(TrainStationsCacheKey, out ILookup<string, TrainStation> trainStations))
        {
            return trainStations;
        }

        return null;
    }
}
