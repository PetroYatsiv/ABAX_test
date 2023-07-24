using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services
{
    public interface ITrainStationFetcher
    {
        Task<Dictionary<string, TrainStation>> GetStationsFromApiAsync();
    }
}
