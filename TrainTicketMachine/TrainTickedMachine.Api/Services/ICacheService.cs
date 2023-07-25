using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services;

public interface ICacheService
{
    void SetCache(ILookup<string, TrainStation> trainStations);
    Task SetCacheFromApi();
    void ClearCache();
}
