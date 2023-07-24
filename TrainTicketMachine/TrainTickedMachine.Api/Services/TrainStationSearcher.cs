using Microsoft.Extensions.Caching.Distributed;

namespace TrainTickedMachine.Api.Services
{
    public class TrainStationSearcher : ITrainStationSearcher
    {
        private readonly ILogger<TrainStationSearcher> _logger;
        private readonly IDistributedCache _cache;

        public TrainStationSearcher(ILogger<TrainStationSearcher> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        //public async Task<TrainStation> GetStationFromCacheAsync(string stationCode)
        //{
        //    var trainStation = await _cache.GetStringAsync(stationCode);

        //    if (trainStation is null)
        //    {
        //        _logger.LogInformation($"Station with code {stationCode} not found in cache");
        //        return null;
        //    }

        //    return JsonSerializer.Deserialize<TrainStation>(trainStation);
        //}
    }
}
