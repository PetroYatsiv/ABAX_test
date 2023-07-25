using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services;

public interface ICacheService
{
    Task<ILookup<string, TrainStation>>? SetCache();
    void SetCache(ILookup<string, TrainStation> trainStations);
}
