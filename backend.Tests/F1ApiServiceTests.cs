using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Pitwall.Services;
using Xunit;

namespace Pitwall.Tests;

public class F1ApiServiceTests
{
    // =========================================================================
    // Validation — Season
    // =========================================================================

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("1800")]
    [InlineData("9999")]
    public async Task GetRaces_InvalidSeason_ThrowsArgumentException(string season)
    {
        var service = BuildService(EmptyRacesJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetRaces(season));
    }

    [Fact]
    public async Task GetRaces_SeasonTooEarly_ThrowsArgumentException()
    {
        var service = BuildService(EmptyRacesJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetRaces("0"));
    }

    [Fact]
    public async Task GetRaces_SeasonTooFarFuture_ThrowsArgumentException()
    {
        var service = BuildService(EmptyRacesJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetRaces("9999"));
    }

    [Fact]
    public async Task GetRaces_NonNumericSeason_ThrowsArgumentException()
    {
        var service = BuildService(SampleRacesJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetRaces("20XX"));
    }

    [Fact]
    public async Task GetRaces_SeasonAtLowerBound_Succeeds()
    {
        var service = BuildService(SampleRacesJson());
        var response = await service.GetRaces("1950");
        Assert.NotNull(response);
    }

    
    [Fact]
    public async Task GetRaces_SeasonAtCurrentYear_Succeeds()
    {
        var service = BuildService(SampleRacesJson());
        var response = await service.GetRaces(DateTime.Now.Year.ToString());
        
        Assert.Equal(DateTime.Now.Year.ToString(), response.MRData.RaceTable.Season);
    }

    
    [Fact]
    public async Task GetDrivers_InvalidSeason_ThrowsArgumentException()
    {
        var service = BuildService(SampleDriversJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetDrivers("20XX"));
    }

    [Fact]
    public async Task GetConstructors_InvalidSeason_ThrowsArgumentException()
    {
        var service = BuildService(SampleConstructorsJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetConstructors("20XX"));
    }

    [Fact]
    public async Task GetResults_InvalidSeason_ThrowsArgumentException()
    {
        var service = BuildService(SampleResultsJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetResults("20XX"));
    }
    

    // =========================================================================
    // Validation — Round
    // =========================================================================

    
    [Fact]
    public async Task GetResults_NonNumericRound_ThrowsArgumentException()
    {
        var service = BuildService(SampleResultsJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetResults("2026", "RoundX"));
    }

    [Fact]
    public async Task GetResults_RoundZero_ThrowsArgumentException()
    {
        var service = BuildService(SampleResultsJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetResults("2026", "0"));
    }

    [Fact]
    public async Task GetResults_RoundNegative_ThrowsArgumentException()
    {
        var service = BuildService(SampleResultsJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetResults("2026", "-1"));
    }

    [Fact]
    public async Task GetResults_RoundOverMax_ThrowsArgumentException()
    {
        var service = BuildService(SampleResultsJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetResults("2026", "27"));
    }

    [Fact]
    public async Task GetResults_NullRound_Succeeds()
    {
        var service = BuildService(SampleResultsJson());
        var response = await service.GetResults("2026", null);
        Assert.NotNull(response);

    }

    [Fact]
    public async Task GetResults_ValidRound_Succeeds()
    {
        var service = BuildService(SampleResultsJson());
        var response = await service.GetResults("2026", "1");
        Assert.NotNull(response);
    }

    [Fact]
    public async Task GetPitStops_NullRound_ThrowsArgumentException()
    {
        var service = BuildService(SampleResultsJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetPitStops("2026", null));
    }

    [Fact]
    public async Task GetPitStops_ValidRound_Succeeds()
    {
        var service = BuildService(SampleResultsJson());
        var response = await service.GetPitStops("2026", "1");
        Assert.NotNull(response);
    }
    

    [Fact]
    public async Task GetLaps_NullRound_ThrowsArgumentException()
    {
        var service = BuildService(SampleResultsJson());
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetLaps("2026", null));
    }


    // =========================================================================
    // Caching
    // =========================================================================

    [Fact]
    public async Task GetRaces_CalledTwiceWithSameSeason_OnlyMakesOneHttpRequest()
    {
        var handler = new CountingHttpHandler(EmptyRacesJson());
        var service = BuildService(handler);

        await service.GetRaces("2025");
        await service.GetRaces("2025");

        Assert.Equal(1, handler.CallCount);
    }

    /*
    [Fact]
    public async Task GetRaces_DifferentSeasons_MakesTwoHttpRequests()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetSeasons_CalledTwice_OnlyMakesOneHttpRequest()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetCircuits_CalledTwice_OnlyMakesOneHttpRequest()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetResults_SameSeasonAndRound_OnlyMakesOneHttpRequest()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetResults_DifferentRounds_MakesTwoHttpRequests()
    {
        throw new NotImplementedException();
    }
    */

    // =========================================================================
    // GetLatestRace logic
    // =========================================================================

    [Fact]
    public async Task GetLatestRace_WithPastAndFutureRaces_ReturnsMostRecentPastRace()
    {
        var past1  = DateTime.UtcNow.AddDays(-30).ToString("yyyy-MM-ddTHH:mm:ss");
        var past2  = DateTime.UtcNow.AddDays(-7).ToString("yyyy-MM-ddTHH:mm:ss");
        var future = DateTime.UtcNow.AddDays(14).ToString("yyyy-MM-ddTHH:mm:ss");

        var json = $$"""
            {
              "MRData": {
                "RaceTable": {
                  "Season": "2025",
                  "Races": [
                    { "Season": "2025", "Round": "1", "RaceName": "Bahrain GP",       "Date": "{{past1}}"  },
                    { "Season": "2025", "Round": "2", "RaceName": "Saudi Arabian GP", "Date": "{{past2}}"  },
                    { "Season": "2025", "Round": "3", "RaceName": "Australian GP",    "Date": "{{future}}" }
                  ]
                }
              }
            }
            """;

        var service = BuildService(json);
        var result = await service.GetLatestRace("2025");

        Assert.Equal("Saudi Arabian GP", result.RaceName);
    }

    /*
    [Fact]
    public async Task GetLatestRace_AllRacesInFuture_ReturnsFirstUpcoming()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetLatestRace_AllRacesInPast_ReturnsLastOne()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetLatestRace_SingleRaceInPast_ReturnsThatRace()
    {
        throw new NotImplementedException();
    }
    */

    [Fact]
    public async Task GetLatestRace_EmptyRaceList_ThrowsInvalidOperationException()
    {
        var service = BuildService(EmptyRacesJson());
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetLatestRace("2026"));
    }

    /*
    [Fact]
    public async Task GetLatestRace_NoSeasonProvided_UsesCurrentYear()
    {
        throw new NotImplementedException();
    }
    */

    // =========================================================================
    // HTTP / deserialization
    // =========================================================================

    /*
    [Fact]
    public async Task GetRaces_HttpReturns500_ThrowsHttpRequestException()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetRaces_HttpReturns404_ThrowsHttpRequestException()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetRaces_MalformedJson_ThrowsJsonException()
    {
        throw new NotImplementedException();
    }
    */

    [Fact]
    public async Task GetRaces_ValidResponse_ReturnsDeserializedData()
    {
        var service = BuildService(SampleRacesJson());
        var response = await service.GetRaces("2026");

        Assert.NotNull(response.MRData.RaceTable.Races);
        Assert.Single(response.MRData.RaceTable.Races);
        Assert.Equal("Bahrain GP", response.MRData.RaceTable.Races[0].RaceName);
    }

    [Fact]
    public async Task GetRaces_NoRacesInSeason_ReturnsEmptyList()
    {
        var service = BuildService(EmptyRacesJson());
        var response = await service.GetRaces("2026");
        Assert.Empty(response.MRData.RaceTable.Races);
    }

    // =========================================================================
    // URL construction
    // =========================================================================

    /*
    [Fact]
    public async Task GetResults_WithRound_UrlContainsSeasonAndRound()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetResults_WithoutRound_UrlDoesNotContainRound()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetSprint_WithRound_UrlContainsSprint()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetQualifying_WithRound_UrlContainsQualifying()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task GetLaps_ValidArgs_UrlContainsLimitParam()
    {
        throw new NotImplementedException();
    }
    */

    // =========================================================================
    // Helpers
    // =========================================================================

    private static F1ApiService BuildService(string json)
        => BuildService(new CountingHttpHandler(json));

    private static F1ApiService BuildService(CountingHttpHandler handler)
    {
        var httpClient = new HttpClient(handler);
        var cache      = new MemoryCache(new MemoryCacheOptions());
        var logger     = NullLogger<F1ApiService>.Instance;
        return new F1ApiService(httpClient, logger, cache);
    }

    private static string EmptyRacesJson() => """
        {
          "MRData": {
            "RaceTable": {
              "Season": "2026",
              "Races": []
            }
          }
        }
        """;

    private static string SampleRacesJson() => """
        {
          "MRData": {
            "RaceTable": {
              "Season": "2026",
              "Races": [
                { "Season": "2026", "Round": "1", "RaceName": "Bahrain GP", "Date": "2026-03-27T15:00:00Z" }
              ]
            }
          }
        }
        """;

    private static string SampleDriversJson() => """
        {
          "MRData": {
            "DriverTable": {
              "Season": "2026",
              "Drivers": [
                { "DriverId": "hamilton", "GivenName": "Lewis", "FamilyName": "Hamilton" },
                { "DriverId": "verstappen", "GivenName": "Max", "FamilyName": "Verstappen" }
              ]
            }
          }
        }
        """;

    private static string SampleConstructorsJson() => """
        {
          "MRData": {
            "ConstructorTable": {
              "Season": "2026",
              "Constructors": [
                { "ConstructorId": "mclaren", "Name": "McLaren" },
                { "ConstructorId": "mercedes", "Name": "Mercedes" }
              ]
            }
          }
        }
        """;

      private static string SampleResultsJson() => """
        {
          "MRData": {
            "raceTable": {
              "season": "2026",
              "round": "3",
              "races": [
                {
                  "season": "2026",
                  "round": "3",
                  "raceName": "Japanese Grand Prix",
                  "results": [
                    {
                      "number": "12", "positionText": "1", "points": "25",
                      "driver": { "driverId": "antonelli", "givenName": "Andrea Kimi", "familyName": "Antonelli" },
                      "constructor": { "constructorId": "mercedes", "name": "Mercedes" }
                    },
                    {
                      "number": "81", "positionText": "2", "points": "18",
                      "driver": { "driverId": "piastri", "givenName": "Oscar", "familyName": "Piastri" },
                      "constructor": { "constructorId": "mclaren", "name": "McLaren" }
                    },
                    {
                      "number": "16", "positionText": "3", "points": "15",
                      "driver": { "driverId": "leclerc", "givenName": "Charles", "familyName": "Leclerc" },
                      "constructor": { "constructorId": "ferrari", "name": "Ferrari" }
                    }
                  ]
                }
              ]
            }
          }
        }
        """;

    private sealed class CountingHttpHandler : HttpMessageHandler
    {
        private readonly string _json;
        public int CallCount { get; private set; }

        public CountingHttpHandler(string json) => _json = json;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_json, Encoding.UTF8, "application/json")
            });
        }
    }
}
