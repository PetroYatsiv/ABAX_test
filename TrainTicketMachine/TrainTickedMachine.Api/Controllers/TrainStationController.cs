using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TrainTickedMachine.Api.Services;

namespace TrainTickedMachine.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainStationController : ControllerBase
{
    private readonly ILogger<TrainStationController> _logger;
    private readonly ITrainStationFetcher _trainStationFetcher;
    private readonly ITrainStationSearcher _trainStationSearcher;
    private readonly ICacheService _cacheService;
    public TrainStationController(
        ILogger<TrainStationController> logger, 
        ITrainStationFetcher stationFetcher,
        ITrainStationSearcher trainStationSearcher,
        ICacheService cacheService)
    {
        _logger = logger;
        _trainStationFetcher = stationFetcher;
        _trainStationSearcher = trainStationSearcher;
        _cacheService = cacheService;
    }

    /// <summary>
    /// Getting train stations which matches with the name
    /// </summary>
    /// <param name="name"></param>
    /// <returns>A collection of train stations that match the given name.</returns>
    [HttpGet("search/{name}")]
    public async Task<IActionResult> SearchStation(string name)
    {
        //try to get from api for new data, if not found, try to get from cache
        var stations = await _trainStationFetcher.GetStationsFromApiAsync();
        if (stations != null)
        {
            _cacheService.ClearCache();
            _cacheService.SetCache(stations);

            var response = _trainStationSearcher.SearchStation(name, stations);
            return Ok(response);
        }
        else
        {
            //try to get from cache
            stations = await _trainStationFetcher.GetStationsFromCacheAsync();
            if (stations != null)
            {
                var response = _trainStationSearcher.SearchStation(name, stations);
                return Ok(response);
            }
        }

        return NotFound();
    }
}
