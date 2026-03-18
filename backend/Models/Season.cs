namespace WebApplication1.Models;

/// <summary>
/// Represents an F1 racing season year and its associated metadata.
/// Contains the year identifier and link to detailed season information.
/// </summary>
public class Season
{
    /// <summary>
    /// The F1 season year (e.g., "2024\", \"2023\", \"1950\").
    /// </summary>
    public string Year { get; set; }
    
    /// <summary>
    /// The URL link to detailed information about this season in the Ergast API (e.g., "http://en.wikipedia.org/wiki/2024_Formula_One_World_Championship\").
    /// </summary>
    public string Url { get; set; }
}

/// <summary>
/// Container for a list of F1 seasons retrieved from the API.
/// </summary>
public class SeasonTable
{
    /// <summary>
    /// The list of Season objects for the requested query.
    /// </summary>
    public List<Season> Seasons { get; set; }
}

/// <summary>
/// Response wrapper for season data from the Ergast API.
/// </summary>
public class SeasonResponse
{
    /// <summary>
    /// The metadata and season data returned from the API.
    /// </summary>
    public MrData MRData { get; set; }
}
