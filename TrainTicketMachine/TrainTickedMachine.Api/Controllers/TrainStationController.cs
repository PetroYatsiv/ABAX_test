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
    public TrainStationController(ILogger<TrainStationController> logger, ITrainStationFetcher stationFetcher)
    {
        _logger = logger;
        _trainStationFetcher = stationFetcher;
    }

    /// <summary>
    /// Getting train stations which matches with the name
    /// </summary>
    /// <param name="name"></param>
    /// <returns>A collection of train stations that match the given name.</returns>
    [HttpGet("search/{name}")]
    public async Task<IActionResult> SearchStation(string name)
    {
        var stations = await _trainStationFetcher.GetStationsFromApiAsync();
        return Ok(stations);
    }

}
