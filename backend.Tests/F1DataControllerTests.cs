using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Pitwall.Controllers;
using Pitwall.Models;
using Pitwall.Services;
using Xunit;

namespace Pitwall.Tests;

public class F1DataControllerTests
{
    // =========================================================================
    // GetSeasons
    // =========================================================================

    [Fact]
    public async Task GetSeasons_ServiceReturnsData_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetSeasons();
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetSeasons_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetSeasons();
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetCircuits
    // =========================================================================

    [Fact]
    public async Task GetCircuits_ServiceReturnsData_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetCircuits();
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetCircuits_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetCircuits();
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetRaces
    // =========================================================================

    [Fact]
    public async Task GetRaces_ValidSeason_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetRaces("2026");
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetRaces_InvalidSeason_ReturnsBadRequest()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowArgument = true });
        var result = await controller.GetRaces("bad");
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetRaces_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetRaces("2026");
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetConstructors
    // =========================================================================

    [Fact]
    public async Task GetConstructors_ValidSeason_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetConstructors("2026");
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetConstructors_InvalidSeason_ReturnsBadRequest()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowArgument = true });
        var result = await controller.GetConstructors("bad");
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetConstructors_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetConstructors("2026");
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetDrivers
    // =========================================================================

    [Fact]
    public async Task GetDrivers_ValidSeason_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetDrivers("2026");
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetDrivers_InvalidSeason_ReturnsBadRequest()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowArgument = true });
        var result = await controller.GetDrivers("bad");
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetDrivers_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetDrivers("2026");
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetResults
    // =========================================================================

    [Fact]
    public async Task GetResults_ValidSeasonNoRound_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetResults("2026");
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetResults_ValidSeasonAndRound_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetResults("2026", "1");
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetResults_InvalidInput_ReturnsBadRequest()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowArgument = true });
        var result = await controller.GetResults("bad");
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetResults_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetResults("2026");
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetQualifying
    // =========================================================================

    [Fact]
    public async Task GetQualifying_ValidSeason_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetQualifying("2026");
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetQualifying_InvalidInput_ReturnsBadRequest()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowArgument = true });
        var result = await controller.GetQualifying("bad");
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetQualifying_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetQualifying("2026");
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetSprint
    // =========================================================================

    [Fact]
    public async Task GetSprint_ValidSeason_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetSprint("2026");
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetSprint_InvalidInput_ReturnsBadRequest()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowArgument = true });
        var result = await controller.GetSprint("bad");
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetSprint_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetSprint("2026");
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetPitStops
    // =========================================================================

    [Fact]
    public async Task GetPitStops_ValidInput_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetPitStops("2026", "1");
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetPitStops_InvalidInput_ReturnsBadRequest()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowArgument = true });
        var result = await controller.GetPitStops("2026", "bad");
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetPitStops_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetPitStops("2026", "1");
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetLaps
    // =========================================================================

    [Fact]
    public async Task GetLaps_ValidInput_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetLaps("2026", "1");
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetLaps_InvalidInput_ReturnsBadRequest()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowArgument = true });
        var result = await controller.GetLaps("2026", "bad");
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetLaps_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetLaps("2026", "1");
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetDriverStandings
    // =========================================================================

    [Fact]
    public async Task GetDriverStandings_ValidSeason_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetDriverStandings("2026");
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetDriverStandings_InvalidSeason_ReturnsBadRequest()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowArgument = true });
        var result = await controller.GetDriverStandings("bad");
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetDriverStandings_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetDriverStandings("2026");
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetStatus
    // =========================================================================

    [Fact]
    public async Task GetStatus_ServiceReturnsData_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetStatus();
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetStatus_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetStatus();
        AssertIs500(result.Result);
    }

    // =========================================================================
    // GetLatestRace
    // =========================================================================

    [Fact]
    public async Task GetLatestRace_ServiceReturnsData_ReturnsOk()
    {
        var controller = BuildController(new FakeF1ApiService());
        var result = await controller.GetLatestRace();
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetLatestRace_ServiceThrows_Returns500()
    {
        var controller = BuildController(new FakeF1ApiService { ThrowGeneral = true });
        var result = await controller.GetLatestRace();
        AssertIs500(result.Result);
    }

    // =========================================================================
    // Helpers
    // =========================================================================

    private static F1DataController BuildController(IF1ApiService service)
        => new F1DataController(service, NullLogger<F1DataController>.Instance);

    private static void AssertIs500(ActionResult? result)
    {
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    private sealed class FakeF1ApiService : IF1ApiService
    {
        public bool ThrowArgument { get; init; }
        public bool ThrowGeneral  { get; init; }

        private void MaybeThrow()
        {
            if (ThrowArgument) throw new ArgumentException("invalid input", "param");
            if (ThrowGeneral)  throw new InvalidOperationException("simulated failure");
        }

        private static F1ApiResponse EmptyResponse() => new F1ApiResponse();
        private static Race          EmptyRace()     => new Race { RaceName = "Test GP", Date = DateTime.UtcNow.AddDays(-1) };

        public Task<F1ApiResponse> GetSeasons()                                      { MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<F1ApiResponse> GetCircuits()                                     { MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<F1ApiResponse> GetRaces(string season)                           { MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<F1ApiResponse> GetConstructors(string season, string? round = null) { MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<F1ApiResponse> GetDrivers(string season)                         { MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<F1ApiResponse> GetResults(string season, string? round = null)   { MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<F1ApiResponse> GetSprint(string season, string? round = null)    { MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<F1ApiResponse> GetQualifying(string season, string? round = null){ MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<F1ApiResponse> GetPitStops(string season, string round)          { MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<F1ApiResponse> GetLaps(string season, string round)              { MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<F1ApiResponse> GetDriverStandings(string season, string? round = null) { MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<F1ApiResponse> GetStatuses()                                     { MaybeThrow(); return Task.FromResult(EmptyResponse()); }
        public Task<Race>          GetLatestRace(string? season = null)              { MaybeThrow(); return Task.FromResult(EmptyRace()); }
    }
}
