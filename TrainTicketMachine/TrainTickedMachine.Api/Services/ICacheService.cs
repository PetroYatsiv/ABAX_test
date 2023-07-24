using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services;

public interface ICacheService
{
    Task<Dictionary<string, TrainStation>>? SetCache();
    void SetCache(Dictionary<string, TrainStation> trainStations);
}
