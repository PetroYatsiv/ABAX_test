using Microsoft.Extensions.Caching.Distributed;
using TrainTickedMachine.Api.Models.Dtos;
using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services
{
    public class TrainStationSearcher : ITrainStationSearcher
    {
        private readonly ILogger<TrainStationSearcher> _logger;

        public TrainStationSearcher(ILogger<TrainStationSearcher> logger)
        {
            _logger = logger;
        }

        public ResponseSearchStationsDto SearchStation(string name, ILookup<string, TrainStation> stations)
        {
            var resultStations = stations
                .Where(group => group.Key.StartsWith(name, StringComparison.OrdinalIgnoreCase))
            .SelectMany(group => group).Select(x => x.StationName).ToList();

            var nextLetters = resultStations
                .Select(station => station[name.Length..].FirstOrDefault())
                .Distinct()
                .ToList();
            var result = new ResponseSearchStationsDto
            {
                Stations = resultStations,
                NextLetters = nextLetters
            };

            _logger.LogInformation($"SearchStation: {name} - {resultStations.Count} results");

            return result;
        }
    }
}
