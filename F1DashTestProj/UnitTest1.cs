using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Services;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace WebApplication1.Tests;



public class F1DataControllerTests
{
    [Fact]
    public async Task GetSeasons_ReturnsOkResult_WhenSeasonsExist()
    { 
        var mockService = new Mock<IF1ApiService>();
        var mockLogger = new Mock<ILogger<F1DataController>>();
        var response = new F1ApiResponse 
        { 
            MRData = new MrData { Total = "1", Limit = "1", Offset = "0" } 
        };
        mockService.Setup(service => service.GetSeasons())
                   .ReturnsAsync(response);
        
        var controller = new F1DataController(mockService.Object, mockLogger.Object);
        var result = await controller.GetSeasons();
        
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        var returnValue = Assert.IsType<F1ApiResponse>(okResult.Value);
        Assert.NotNull(returnValue.MRData);
        Assert.Equal("1", returnValue.MRData.Total);
    }
    [Fact] 
    public async Task GetSeasons_ReturnsOkResult_WhenSeasonsDontExist()
    {
        var mockService = new Mock<IF1ApiService>();
        var mockLogger = new Mock<ILogger<F1DataController>>();
        var response = new F1ApiResponse 
        { 
            MRData = new MrData { Total = "0", Limit = "1", Offset = "0" } 
        };
        mockService.Setup(service => service.GetSeasons())
                   .ReturnsAsync(response);
        
        var controller = new F1DataController(mockService.Object, mockLogger.Object);
        var result = await controller.GetSeasons();
        
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        var returnValue = Assert.IsType<F1ApiResponse>(okResult.Value);
        Assert.NotNull(returnValue.MRData);
        Assert.Equal("0", returnValue.MRData.Total);
    }

    [Fact]
    public async Task GetCircuits_ReturnsOkResult_WhenCircuitsExist()
    {
        var mockService = new Mock<IF1ApiService>();
        var mockLogger = new Mock<ILogger<F1DataController>>();
        var response = new F1ApiResponse 
        { 
            MRData = new MrData { Total = "1", Limit = "1", Offset = "0" } 
        };
        mockService.Setup(service => service.GetCircuits())
                   .ReturnsAsync(response);
        
        var controller = new F1DataController(mockService.Object, mockLogger.Object);
        var result = await controller.GetCircuits();
        
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        var returnValue = Assert.IsType<F1ApiResponse>(okResult.Value);
        
        Assert.IsType<OkObjectResult>(okResult);
    }
    [Fact] 
    public async Task GetCircuits_ReturnsOkResult_WhenCircuitsDontExist()
    {
        var mockService = new Mock<IF1ApiService>();
        var mockLogger = new Mock<ILogger<F1DataController>>();
        var response = new F1ApiResponse 
        { 
            MRData = new MrData { Total = "0", Limit = "1", Offset = "0" } 
        };
        mockService.Setup(service => service.GetCircuits())
                   .ReturnsAsync(response);
        
        var controller = new F1DataController(mockService.Object, mockLogger.Object);
        var result = await controller.GetCircuits();

        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        var returnValue = Assert.IsType<F1ApiResponse>(okResult.Value);
        
        Assert.IsType<OkObjectResult>(okResult);
        
    }


    [Fact]
    public async Task GetRaces_ReturnsOkResult_WhenRacesExist()
    {
        var mockService = new Mock<IF1ApiService>();
        var mockLogger = new Mock<ILogger<F1DataController>>();
        var response = new F1ApiResponse 
        { 
            MRData = new MrData { Total = "1", Limit = "1", Offset = "0" } 
        };
        mockService.Setup(service => service.GetRaces(It.IsAny<string>()))
                   .ReturnsAsync(response);
        
        var controller = new F1DataController(mockService.Object, mockLogger.Object);
        var result = await controller.GetRaces("2024");
        
        Console.WriteLine("\n=== GetRaces_ReturnsOkResult_WhenRacesExist ===");
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Console.WriteLine($"Status Code: {okResult.StatusCode}");
        
        var returnValue = Assert.IsType<F1ApiResponse>(okResult.Value);
        Console.WriteLine($"Returned Offset: {returnValue}");
        
        Assert.IsType<OkObjectResult>(okResult);
        
    }
    [Fact] 
    public async Task GetRaces_ReturnsOkResult_WhenRacesDontExist()
    {
        var mockService = new Mock<IF1ApiService>();
        var mockLogger = new Mock<ILogger<F1DataController>>();
        var response = new F1ApiResponse 
        { 
            MRData = new MrData { Total = "0", Limit = "1", Offset = "0" } 
        };
        mockService.Setup(service => service.GetRaces(It.IsAny<string>()))
                   .ReturnsAsync(response);
        
        var controller = new F1DataController(mockService.Object, mockLogger.Object);
        var result = await controller.GetRaces("2024");

        Console.WriteLine("\n=== GetRaces_ReturnsOkResult_WhenRacesDontExist ===");
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Console.WriteLine($"Status Code: {okResult.StatusCode}");
        
        var returnValue = Assert.IsType<F1ApiResponse>(okResult.Value);
        Console.WriteLine($"Returned Total: {returnValue.MRData.Total}");
        Console.WriteLine($"Returned Limit: {returnValue.MRData.Limit}");
        Console.WriteLine($"Returned Offset: {returnValue.MRData.Offset}");
        
        Assert.IsType<OkObjectResult>(okResult);
        Console.WriteLine("✓ Test PASSED\n");
    }


    // [Fact]
    // public async Task GetConstructors_ReturnsOkResult_WhenConstructorsExist()
    // {

    // }
    // [Fact] 
    //     public async Task GetConstructors_ReturnsOkResult_WhenConstructorsDontExist()
    // {

    // }


    // [Fact]
    // public async Task GetDrivers_ReturnsOkResult_WhenDriversExist()
    // {

    // }
    // [Fact] 
    //     public async Task GetDrivers_ReturnsOkResult_WhenDriversDontExist()
    // {

    // }

    // [Fact]
    // public async Task GetResults_ReturnsOkResult_WhenResultsExist()
    // {

    // }
    // [Fact] 
    //     public async Task GetResults_ReturnsOkResult_WhenResultsDontExist()
    // {

    // }

    // [Fact]
    // public async Task GetQualifying_ReturnsOkResult_WhenQualifyingExists()
    // {

    // }
    // [Fact] 
    //     public async Task GetQualifying_ReturnsOkResult_WhenQualifyingDoesntExist()
    // {

    // }

    // [Fact]
    // public async Task GetSprint_ReturnsOkResult_WhenSprintsExist()
    // {

    // }
    // [Fact] 
    //     public async Task GetSprint_ReturnsOkResult_WhenSprintsDontExist()
    // {

    // }

    // [Fact]
    // public async Task GetPitStops_ReturnsOkResult_WhenPitStopsExist()
    // {

    // }
    // [Fact] 
    //     public async Task GetPitStops_ReturnsOkResult_WhenPitStopsDontExist()
    // {

    // }

    // [Fact]
    // public async Task GetLaps_ReturnsOkResult_WhenLapsExist()
    // {

    // }
    // [Fact] 
    //     public async Task GetLaps_ReturnsOkResult_WhenLapsDontExist()
    // {

    // }

    // [Fact]
    // public async Task GetDriverStandings_ReturnsOkResult_WhenDriverStandingsExist()
    // {

    // }
    // [Fact] 
    //     public async Task GetDriverStandings_ReturnsOkResult_WhenDriverStandingsDontExist()
    // {

    // }

    // [Fact]
    // public async Task GetConstructorStandings_ReturnsOkResult_WhenConstructorStandingsExist()
    // {

    // }
    // [Fact] 
    //     public async Task GetConstructorStandings_ReturnsOkResult_WhenConstructorStandingsDontExist()
    // {

    // }


    // [Fact]
    // public async Task GetStatusCodes_ReturnsOkResult_WhenStatusCodesExist()
    // {

    // }
    // [Fact] 
    //     public async Task GetStatusCodes_ReturnsOkResult_WhenStatusCodesDontExist()
    // {

    // }

    }
