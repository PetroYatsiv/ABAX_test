using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services;

public class TrainStationFetcher : ITrainStationFetcher
{
    private readonly ILogger<TrainStationFetcher> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public TrainStationFetcher(
        ILogger<TrainStationFetcher> logger, 
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
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

        var stationDictionary = trainStations.ToLookup(station => station.StationName);
        
        return stationDictionary;
    }
}
