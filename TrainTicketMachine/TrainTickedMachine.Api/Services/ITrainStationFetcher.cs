using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services
{
    public interface ITrainStationFetcher
    {
        Task<ILookup<string, TrainStation>> GetStationsFromApiAsync();
        Task<ILookup<string, TrainStation>?> GetStationsFromCacheAsync();
    }
}
