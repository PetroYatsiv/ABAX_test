using TrainTickedMachine.Api.Models.Dtos;
using TrainTicketMachine.Domain.Entities;

namespace TrainTickedMachine.Api.Services
{
    public interface ITrainStationSearcher
    {
        ResponseSearchStationsDto SearchStation(string name, ILookup<string, TrainStation> stations);
    }
}
