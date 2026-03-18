using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/f1")]
public class F1DataController : ControllerBase
{
    private readonly IF1ApiService _f1ApiService;
    private readonly ILogger<F1DataController> _logger;

    public F1DataController(IF1ApiService f1ApiService, ILogger<F1DataController> logger)
    {
        _f1ApiService = f1ApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get all F1 seasons
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("seasons")]
    public async Task<ActionResult<F1ApiResponse>> GetSeasons()
    {
        try
        {
            var result = await _f1ApiService.GetSeasons();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching seasons");
            return StatusCode(500, new { error = "Failed to fetch seasons" });
        }
    }

    /// <summary>
    /// Get all F1 circuits
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("circuits")]
    public async Task<ActionResult<F1ApiResponse>> GetCircuits()
    {
        try
        {
            var result = await _f1ApiService.GetCircuits();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching circuits");
            return StatusCode(500, new { error = "Failed to fetch circuits" });
        }
    }

    /// <summary>
    /// Get races for a specific season
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("races/{season}")]
    public async Task<ActionResult<F1ApiResponse>> GetRaces(string season)
    {
        try
        {
            var result = await _f1ApiService.GetRaces(season);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching races for season {season}");
            return StatusCode(500, new { error = $"Failed to fetch races for season {season}" });
        }
    }

    /// <summary>
    /// Get constructors for a specific season
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("constructors/{season}")]
    public async Task<ActionResult<F1ApiResponse>> GetConstructors(string season)
    {
        try
        {
            var result = await _f1ApiService.GetConstructors(season);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching constructors for season {season}");
            return StatusCode(500, new { error = $"Failed to fetch constructors for season {season}" });
        }
    }

    /// <summary>
    /// Get drivers for a specific season
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("drivers/{season}")]
    public async Task<ActionResult<F1ApiResponse>> GetDrivers(string season)
    {
        try
        {
            var result = await _f1ApiService.GetDrivers(season);
            Console.WriteLine($"Fetched drivers for season {season}: {result}");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching drivers for season {season}");
            return StatusCode(500, new { error = $"Failed to fetch drivers for season {season}" });
        }
    }

    

    /// <summary>
    /// Get race results for a season or specific round
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("results/{season}/{round?}")]
    public async Task<ActionResult<F1ApiResponse>> GetResults(string season, string? round = null)
    {
        try
        {
            var result = await _f1ApiService.GetResults(season, round);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching results for season {season}, round {round}");
            return StatusCode(500, new { error = "Failed to fetch results" });
        }
    }

    /// <summary>
    /// Get qualifying results for a season or specific round
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("qualifying/{season}/{round?}")]
    public async Task<ActionResult<F1ApiResponse>> GetQualifying(string season, string? round = null)
    {
        try
        {
            var result = await _f1ApiService.GetQualifying(season, round);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching qualifying for season {season}, round {round}");
            return StatusCode(500, new { error = "Failed to fetch qualifying results" });
        }
    }

    /// <summary>
    /// Get sprint results for a season or specific round
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("sprint/{season}/{round?}")]
    public async Task<ActionResult<F1ApiResponse>> GetSprint(string season, string? round = null)
    {
        try
        {
            var result = await _f1ApiService.GetSprint(season, round);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching sprint results for season {season}, round {round}");
            return StatusCode(500, new { error = "Failed to fetch sprint results" });
        }
    }
 
    /// Get pit stops for a specific race
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("pitstops/{season}/{round}")]
    public async Task<ActionResult<F1ApiResponse>> GetPitStops(string season, string round)
    {
        try
        {
            var result = await _f1ApiService.GetPitStops(season, round);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching pit stops for season {season}, round {round}");
            return StatusCode(500, new { error = "Failed to fetch pit stops" });
        }
    }

    /// <summary>
    /// Get lap times for a specific race
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("laps/{season}/{round}")]
    public async Task<ActionResult<F1ApiResponse>> GetLaps(string season, string round)
    {
        try
        {
            var result = await _f1ApiService.GetLaps(season, round);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching laps for season {season}, round {round}");
            return StatusCode(500, new { error = "Failed to fetch lap times" });
        }
    }

    /// <summary>
    /// Get driver championship standings for a season or after a specific round
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("standings/drivers/{season}/{round?}")]
    public async Task<ActionResult<F1ApiResponse>> GetDriverStandings(string season, string? round = null)
    {
        try
        {
            var result = await _f1ApiService.GetDriverStandings(season, round);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching driver standings for season {season}, round {round}");
            return StatusCode(500, new { error = "Failed to fetch driver standings" });
        }
    }

    /// <summary>
    /// Get constructor championship standings for a season or after a specific round
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("standings/constructors/{season}/{round?}")]
    public async Task<ActionResult<F1ApiResponse>> GetConstructorStandings(string season, string? round = null)
    {
        try
        {
            var result = await _f1ApiService.GetConstructorStandings(season, round);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching constructor standings for season {season}, round {round}");
            return StatusCode(500, new { error = "Failed to fetch constructor standings" });
        }
    }

    /// <summary>
    /// Get all F1 status codes
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("status")]
    public async Task<ActionResult<F1ApiResponse>> GetStatus()
    {
        try
        {
            var result = await _f1ApiService.GetStatuses();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching status codes");
            return StatusCode(500, new { error = "Failed to fetch status codes" });
        }
    }

    /// <summary>
    /// Get the latest race (most recent that has occurred, or upcoming)
    /// </summary>
    [EnableCors("AllowAll")]
    [HttpGet("latest-race")]
    public async Task<ActionResult<Race>> GetLatestRace(string? season = null)
    {
        try
        {
            var result = await _f1ApiService.GetLatestRace(season);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching latest race");
            return StatusCode(500, new { error = "Failed to fetch latest race" });
        }
    }
}

