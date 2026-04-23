using Pitwall.Models;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Pitwall.Services;

// Service interface and implementation for interacting with the F1 API
// Used by controllers to fetch data without needing to know API details or handle HTTP requests directly

public interface IF1ApiService
{
    Task<F1ApiResponse> GetSeasons();
    Task<F1ApiResponse> GetCircuits();
    Task<F1ApiResponse> GetRaces(string season);
    Task<F1ApiResponse> GetConstructors(string season, string? round = null);
    Task<F1ApiResponse> GetDrivers(string season);
    Task<F1ApiResponse> GetResults(string season, string? round = null);
    Task<F1ApiResponse> GetSprint(string season, string? round = null);
    Task<F1ApiResponse> GetQualifying(string season, string? round = null);
    Task<F1ApiResponse> GetPitStops(string season, string round);
    Task<F1ApiResponse> GetLaps(string season, string round);
    Task<F1ApiResponse> GetDriverStandings(string season, string? round = null);
    Task<F1ApiResponse> GetStatuses();
    Task<Race> GetLatestRace(string? season = null);
}

public class F1ApiService : IF1ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<F1ApiService> _logger;
    private readonly IMemoryCache _cache;
    private const string BaseUrl = "https://api.jolpi.ca/ergast/f1";
    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan StaticCacheDuration = TimeSpan.FromHours(1);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public F1ApiService(HttpClient httpClient, ILogger<F1ApiService> logger, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _cache = cache;
    }


    private static void ValidateSeason(string season)
    {
        if (string.IsNullOrWhiteSpace(season))
            throw new ArgumentException("Season cannot be null or empty", nameof(season));
        if  (season.Any(char.IsLetter))
            throw new ArgumentException("Season cannot contain letters", nameof(season));
        if (!int.TryParse(season, out int year) || year < 1950 || year > DateTime.Now.Year + 1)
            throw new ArgumentException("Season must be a valid year between 1950 and current year + 1", nameof(season));
        if (season.Length != 4)
            throw new ArgumentException("Season must be a 4-digit year", nameof(season));
    }

    private static void ValidateRound(string? round, bool required = false)
    {
        if (round == null)
        {
            if (required) throw new ArgumentException("Round cannot be null or empty", nameof(round));
            return;
        }
        if (string.IsNullOrWhiteSpace(round))
            throw new ArgumentException("Round cannot be null or empty", nameof(round));
        if (round.Any(char.IsLetter))
            throw new ArgumentException("Round cannot contain letters", nameof(round));
        if (!int.TryParse(round, out int r) || r < 1 || r > 26)
            throw new ArgumentException("Round must be a valid integer between 1 and 26", nameof(round));
    }

    /// <summary>
    /// Gets all seasons from the F1 API
    /// </summary>
     public async Task<F1ApiResponse> GetSeasons()
    {
        try
        {
            string url = $"{BaseUrl}/seasons";
            return await FetchData(url, StaticCacheDuration);
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
            return await FetchData(url, StaticCacheDuration);
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
        ValidateSeason(season);
        try
        {
            string url = $"{BaseUrl}/{season}/races";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching races for season {Season}", season);
            throw;
        }
    }

    /// <summary>
    /// Gets constructors for a specific season
    /// </summary>
    public async Task<F1ApiResponse> GetConstructors(string season, string? round = null)
    {
        ValidateSeason(season);
        ValidateRound(round);
        try
        {
            string url = $"{BaseUrl}/{season}/constructors";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching constructors for season {Season}", season);
            throw;
        }
    }

    /// <summary>
    /// Gets drivers for a specific season
    /// </summary>
    public async Task<F1ApiResponse> GetDrivers(string season)
    {
        ValidateSeason(season);
        try
        {
            string url = $"{BaseUrl}/{season}/drivers";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching drivers for season {Season}", season);
            throw;
        }
    }

    /// <summary>
    /// Gets race results for a season or specific round
    /// </summary>
    public async Task<F1ApiResponse> GetResults(string season, string? round = null)
    {
        ValidateSeason(season);
        ValidateRound(round);
        try
        {
            string url = round != null 
                ? $"{BaseUrl}/{season}/{round}/results/" 
                : $"{BaseUrl}/{season}/results";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching results for season {Season}, round {Round}", season, round);
            throw;
        }
    }

    /// <summary>
    /// Gets sprint results for a season or specific round
    /// </summary>
    public async Task<F1ApiResponse> GetSprint(string season, string? round = null)
    {
        ValidateSeason(season);
        ValidateRound(round);
        try
        {
            string url = round != null 
                ? $"{BaseUrl}/{season}/{round}/sprint" 
                : $"{BaseUrl}/{season}/sprint";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching sprint results for season {Season}, round {Round}", season, round);
            throw;
        }
    }

    /// <summary>
    /// Gets qualifying results for a season or specific round
    /// </summary>
    public async Task<F1ApiResponse> GetQualifying(string season, string? round = null)
    {
        ValidateSeason(season);
        ValidateRound(round);
        try
        {
            string url = round != null 
                ? $"{BaseUrl}/{season}/{round}/qualifying" 
                : $"{BaseUrl}/{season}/qualifying";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching qualifying results for season {Season}, round {Round}", season, round);
            throw;
        }
    }

    /// <summary>
    /// Gets pit stop data for a specific race
    /// </summary>
    public async Task<F1ApiResponse> GetPitStops(string season, string round)
    {
        ValidateSeason(season);
        ValidateRound(round, required: true);
        try
        {
            string url = $"{BaseUrl}/{season}/{round}/pitstops";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching pit stops for season {Season}, round {Round}", season, round);
            throw;
        }
    }

    /// <summary>
    /// Gets lap timing data for a specific race
    /// </summary>
    public async Task<F1ApiResponse> GetLaps(string season, string round)
    {
        ValidateSeason(season);
        ValidateRound(round, required: true);
        try
        {
            string url = $"{BaseUrl}/{season}/{round}/laps?limit=100";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching laps for season {Season}, round {Round}", season, round);
            throw;
        }
    }

    /// <summary>
    /// Gets driver championship standings for a season or after a specific round
    /// </summary>
    public async Task<F1ApiResponse> GetDriverStandings(string season, string? round = null)
    {
        ValidateSeason(season);
        ValidateRound(round);
        try
        {
            string url = round != null 
                ? $"{BaseUrl}/{season}/{round}/driverstandings" 
                : $"{BaseUrl}/{season}/driverstandings";
            return await FetchData(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching driver standings for season {Season}, round {Round}", season, round);
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
            _logger.LogError(ex, "Error fetching latest race for season {Season}", season);
            throw;
        }
    }

    /// <summary>
    /// Private helper method to fetch and deserialize data from the API
    /// </summary>
        private async Task<F1ApiResponse> FetchData(string url, TimeSpan? cacheDuration = null)
    {
        if (_cache.TryGetValue(url, out F1ApiResponse? cached) && cached != null)
        {
            _logger.LogInformation("Cache hit: {Url}", url);
            return cached;
        }

        try
        {
            _logger.LogInformation("Fetching data from: {Url}", url);

            using var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<F1ApiResponse>(jsonContent, JsonOptions);

            if (data == null)
            {
                throw new InvalidOperationException("Failed to deserialize API response");
            }

            _cache.Set(url, data, cacheDuration ?? DefaultCacheDuration);
            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error when fetching {Url}", url);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error when processing response from {Url}", url);
            throw;
        }
    }
}
