using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Pitwall.Models;
using Pitwall.Services;

namespace Pitwall.Controllers;

[ApiController]
[Route("api/f1")]
[EnableRateLimiting("default")]
public class F1DataController : ControllerBase
{
    private readonly IF1ApiService _f1ApiService;
    private readonly ILogger<F1DataController> _logger;

    public F1DataController(IF1ApiService f1ApiService, ILogger<F1DataController> logger)
    {
        _f1ApiService = f1ApiService;
        _logger = logger;
    }

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

    [HttpGet("races/{season}")]
    public async Task<ActionResult<F1ApiResponse>> GetRaces(string season)
    {
        try
        {
            var result = await _f1ApiService.GetRaces(season);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching races");
            return StatusCode(500, new { error = "Failed to fetch races" });
        }
    }

    [HttpGet("constructors/{season}")]
    public async Task<ActionResult<F1ApiResponse>> GetConstructors(string season)
    {
        try
        {
            var result = await _f1ApiService.GetConstructors(season);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching constructors");
            return StatusCode(500, new { error = "Failed to fetch constructors" });
        }
    }

    [HttpGet("drivers/{season}")]
    public async Task<ActionResult<F1ApiResponse>> GetDrivers(string season)
    {
        try
        {
            var result = await _f1ApiService.GetDrivers(season);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching drivers");
            return StatusCode(500, new { error = "Failed to fetch drivers" });
        }
    }

    [HttpGet("results/{season}/{round?}")]
    public async Task<ActionResult<F1ApiResponse>> GetResults(string season, string? round = null)
    {
        try
        {
            var result = await _f1ApiService.GetResults(season, round);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching results");
            return StatusCode(500, new { error = "Failed to fetch results" });
        }
    }

    [HttpGet("qualifying/{season}/{round?}")]
    public async Task<ActionResult<F1ApiResponse>> GetQualifying(string season, string? round = null)
    {
        try
        {
            var result = await _f1ApiService.GetQualifying(season, round);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching qualifying");
            return StatusCode(500, new { error = "Failed to fetch qualifying results" });
        }
    }

    [HttpGet("sprint/{season}/{round?}")]
    public async Task<ActionResult<F1ApiResponse>> GetSprint(string season, string? round = null)
    {
        try
        {
            var result = await _f1ApiService.GetSprint(season, round);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching sprint results");
            return StatusCode(500, new { error = "Failed to fetch sprint results" });
        }
    }

    [HttpGet("pitstops/{season}/{round}")]
    public async Task<ActionResult<F1ApiResponse>> GetPitStops(string season, string round)
    {
        try
        {
            var result = await _f1ApiService.GetPitStops(season, round);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching pit stops");
            return StatusCode(500, new { error = "Failed to fetch pit stops" });
        }
    }

    [HttpGet("laps/{season}/{round}")]
    public async Task<ActionResult<F1ApiResponse>> GetLaps(string season, string round)
    {
        try
        {
            var result = await _f1ApiService.GetLaps(season, round);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching laps");
            return StatusCode(500, new { error = "Failed to fetch lap times" });
        }
    }

    [HttpGet("standings/drivers/{season}/{round?}")]
    public async Task<ActionResult<F1ApiResponse>> GetDriverStandings(string season, string? round = null)
    {
        try
        {
            var result = await _f1ApiService.GetDriverStandings(season, round);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching driver standings");
            return StatusCode(500, new { error = "Failed to fetch driver standings" });
        }
    }

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
