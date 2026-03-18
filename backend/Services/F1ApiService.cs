using WebApplication1.Models;
using System.Text.Json;

namespace WebApplication1.Services;

// Service interface and implementation for interacting with the F1 API
// Used by controllers to fetch data without needing to know API details or handle HTTP requests directly

public interface IF1ApiService
{
    Task<F1ApiResponse> GetSeasons();
    Task<F1ApiResponse> GetCircuits();
    Task<F1ApiResponse> GetRaces(string season);
    Task<F1ApiResponse> GetConstructors(string season);
    Task<F1ApiResponse> GetDrivers(string season);
    Task<F1ApiResponse> GetResults(string season, string? round = null);
    Task<F1ApiResponse> GetSprint(string season, string? round = null);
    Task<F1ApiResponse> GetQualifying(string season, string? round = null);
    Task<F1ApiResponse> GetPitStops(string season, string round);
    Task<F1ApiResponse> GetLaps(string season, string round);
    Task<F1ApiResponse> GetDriverStandings(string season, string? round = null);
    Task<F1ApiResponse> GetConstructorStandings(string season, string? round = null);
    Task<F1ApiResponse> GetStatuses();
    Task<Race> GetLatestRace(string? season = null);
}

public class F1ApiService : IF1ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<F1ApiService> _logger;
    private const string BaseUrl = "https://api.jolpi.ca/ergast/f1";
    
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public F1ApiService(HttpClient httpClient, ILogger<F1ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Gets all seasons from the F1 API
    /// </summary>
    public async Task<F1ApiResponse> GetSeasons()
    {
        try
        {
            string url = $"{BaseUrl}/seasons";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching seasons");
            throw;
        }
    }

    /// <summary>
    /// Gets all circuits from the F1 API
    /// </summary>
    public async Task<F1ApiResponse> GetCircuits()
    {
        try
        {
            string url = $"{BaseUrl}/circuits";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching circuits");
            throw;
        }
    }

    /// <summary>
    /// Gets races for a specific season
    /// </summary>
    public async Task<F1ApiResponse> GetRaces(string season)
    {
        if (string.IsNullOrWhiteSpace(season))
            throw new ArgumentException("Season cannot be null or empty", nameof(season));
        if (!int.TryParse(season, out _))
            throw new ArgumentException("Season must be a valid year", nameof(season)); 
        try
        {
            string url = $"{BaseUrl}/{season}/races";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching races for season {season}");
            throw;
        }
    }

    /// <summary>
    /// Gets constructors for a specific season
    /// </summary>
    public async Task<F1ApiResponse> GetConstructors(string season)

    
    {
        if (string.IsNullOrWhiteSpace(season))
            throw new ArgumentException("Season cannot be null or empty", nameof(season));
        
        try
        {
            string url = $"{BaseUrl}/{season}/constructors";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching constructors for season {season}");
            throw;
        }
    }

    /// <summary>
    /// Gets drivers for a specific season
    /// </summary>
    public async Task<F1ApiResponse> GetDrivers(string season)
    {
        if (string.IsNullOrWhiteSpace(season))
            throw new ArgumentException("Season cannot be null or empty", nameof(season));
        
        try
        {
            string url = $"{BaseUrl}/{season}/drivers";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching drivers for season {season}");
            throw;
        }
    }

    /// <summary>
    /// Gets race results for a season or specific round
    /// </summary>
    public async Task<F1ApiResponse> GetResults(string season, string? round = null)
    {
        if (string.IsNullOrWhiteSpace(season))
            throw new ArgumentException("Season cannot be null or empty", nameof(season));
        
        try
        {
            string url = round != null 
                ? $"{BaseUrl}/{season}/results/{round}" 
                : $"{BaseUrl}/{season}/results";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching results for season {season}, round {round}");
            throw;
        }
    }

    /// <summary>
    /// Gets sprint results for a season or specific round
    /// </summary>
    public async Task<F1ApiResponse> GetSprint(string season, string? round = null)
    {
        if (string.IsNullOrWhiteSpace(season))
            throw new ArgumentException("Season cannot be null or empty", nameof(season));
        
        try
        {
            string url = round != null 
                ? $"{BaseUrl}/{season}/{round}/sprint" 
                : $"{BaseUrl}/{season}/sprint";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching sprint results for season {season}, round {round}");
            throw;
        }
    }

    /// <summary>
    /// Gets qualifying results for a season or specific round
    /// </summary>
    public async Task<F1ApiResponse> GetQualifying(string season, string? round = null)
    {
        if (string.IsNullOrWhiteSpace(season))
            throw new ArgumentException("Season cannot be null or empty", nameof(season));
        
        try
        {
            string url = round != null 
                ? $"{BaseUrl}/{season}/{round}/qualifying" 
                : $"{BaseUrl}/{season}/qualifying";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching qualifying results for season {season}, round {round}");
            throw;
        }
    }

    /// <summary>
    /// Gets pit stop data for a specific race
    /// </summary>
    public async Task<F1ApiResponse> GetPitStops(string season, string round)
    {
        if (string.IsNullOrWhiteSpace(season))
            throw new ArgumentException("Season cannot be null or empty", nameof(season));
        if (string.IsNullOrWhiteSpace(round))
            throw new ArgumentException("Round cannot be null or empty", nameof(round));
        
        try
        {
            string url = $"{BaseUrl}/{season}/{round}/pitstops";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching pit stops for season {season}, round {round}");
            throw;
        }
    }

    /// <summary>
    /// Gets lap timing data for a specific race
    /// </summary>
    public async Task<F1ApiResponse> GetLaps(string season, string round)
    {
        if (string.IsNullOrWhiteSpace(season))
            throw new ArgumentException("Season cannot be null or empty", nameof(season));
        if (string.IsNullOrWhiteSpace(round))
            throw new ArgumentException("Round cannot be null or empty", nameof(round));
        
        try
        {
            string url = $"{BaseUrl}/{season}/{round}/laps";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching laps for season {season}, round {round}");
            throw;
        }
    }

    /// <summary>
    /// Gets driver championship standings for a season or after a specific round
    /// </summary>
    public async Task<F1ApiResponse> GetDriverStandings(string season, string? round = null)
    {
        if (string.IsNullOrWhiteSpace(season))
            throw new ArgumentException("Season cannot be null or empty", nameof(season));
        
        try
        {
            string url = round != null 
                ? $"{BaseUrl}/{season}/{round}/driverstandings" 
                : $"{BaseUrl}/{season}/driverstandings";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching driver standings for season {season}, round {round}");
            throw;
        }
    }

    /// <summary>
    /// Gets constructor championship standings for a season or after a specific round
    /// </summary>
    public async Task<F1ApiResponse> GetConstructorStandings(string season, string? round = null)
    {
        if (string.IsNullOrWhiteSpace(season))
            throw new ArgumentException("Season cannot be null or empty", nameof(season));
        
        try
        {
            string url = round != null 
                ? $"{BaseUrl}/{season}/{round}/constructorstandings" 
                : $"{BaseUrl}/{season}/constructorstandings";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching constructor standings for season {season}, round {round}");
            throw;
        }
    }

    /// <summary>
    /// Gets all status codes used in race results
    /// </summary>
    public async Task<F1ApiResponse> GetStatuses()
    {
        try
        {
            string url = $"{BaseUrl}/status";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching status codes");
            throw;
        }
    }

    /// <summary>
    /// Gets the latest race that has taken place (or upcoming if none have occurred)
    /// </summary>
    public async Task<Race> GetLatestRace(string? season = null)
    {
        try
        {
            // Use current year if season not specified
            season ??= DateTime.Now.Year.ToString();
            
            var racesResponse = await GetRaces(season);
            var races = racesResponse.MRData.RaceTable?.Races;
            
            if (races == null || races.Count == 0)
            {
                throw new InvalidOperationException($"No races found for season {season}");
            }
            
            // using linq to find the latest race that has already occurred (or upcoming if none have occurred)
            var latestRace = races
                .Where(r => r.Date <= DateTime.Now)
                .OrderByDescending(r => r.Date)
                .FirstOrDefault() 
                ?? races.OrderBy(r => r.Date).First();
            
            return latestRace;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching latest race for season {season}");
            throw;
        }
    }

    /// <summary>
    /// Private helper method to fetch and deserialize data from the API
    /// </summary>
    private async Task<F1ApiResponse> FetchData(string url)
    {
        try
        {
            _logger.LogInformation($"Fetching data from: {url}");
            
            using var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<F1ApiResponse>(jsonContent, JsonOptions);

            if (data == null)
             {
                throw new InvalidOperationException("Failed to deserialize API response");
            }

            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"HTTP request error when fetching {url}");
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, $"JSON deserialization error when processing response from {url}");
            throw;
        }
    }
}
