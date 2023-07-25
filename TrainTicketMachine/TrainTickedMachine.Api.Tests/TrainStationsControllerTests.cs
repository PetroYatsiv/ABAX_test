using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using TrainTickedMachine.Api.Controllers;
using TrainTickedMachine.Api.Services;
using TrainTicketMachine.Domain.Entities;
using Xunit;
using Xunit.Abstractions;
using static System.Collections.Specialized.BitVector32;

namespace TrainTickedMachine.Api.Tests;

public class TrainStationsControllerTests
{
    private Mock<ILogger<TrainStationController>> _loggerMock;
    private Mock<ITrainStationFetcher> _stationFetcherMock;
    private Mock<ITrainStationSearcher> _trainStationSearcherMock;
    private Mock<ICacheService> _cacheServiceMock;
    private TrainStationController _controller;

    public TrainStationsControllerTests(ITestOutputHelper testOutputHelper)
    {
        _loggerMock = new Mock<ILogger<TrainStationController>>();
        _stationFetcherMock = new Mock<ITrainStationFetcher>();
        _trainStationSearcherMock = new Mock<ITrainStationSearcher>();
        _cacheServiceMock = new Mock<ICacheService>();
        _controller = new TrainStationController(
            _loggerMock.Object,
            _stationFetcherMock.Object,
            _trainStationSearcherMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task SearchStation_WhenStationsFoundInApi_ReturnsOk()
    {
        // Arrange
        var name = "Ab";
        string json = @"
        [
            {
                ""stationCode"": ""ABW"",
                ""stationName"": ""Abbey Wood""
            },
            {
                ""stationCode"": ""ABE"",
                ""stationName"": ""Aber""
            },
            {
                ""stationCode"": ""ACY"",
                ""stationName"": ""Abercynon""
            }
        ]"
        ;
        List<TrainStation> stations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrainStation>>(json);
        ILookup<string, TrainStation> stationLookup = stations.ToLookup(s => s.StationName);
        _stationFetcherMock.Setup(mock => mock.GetStationsFromApiAsync()).ReturnsAsync(stationLookup);

        // Act
        var result = await _controller.SearchStation(name);

        // Assert
        _cacheServiceMock.Verify(mock => mock.ClearCache(), Times.Once);
        _cacheServiceMock.Verify(mock => mock.SetCache(stationLookup), Times.Once);
        _trainStationSearcherMock.Verify(mock => mock.SearchStation(name, stationLookup), Times.Once);
        Assert.IsType<OkObjectResult>(result);
    }
}