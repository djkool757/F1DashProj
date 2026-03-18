namespace WebApplication1.Models;

/// <summary>
/// Represents a single Grand Prix race event with all associated data.
/// Contains information about the race date, location, results, qualifying, pit stops, and lap data.
/// </summary>
public class Race
{
    /// <summary>
    /// The F1 season/year in which this race took place (e.g., "2024").
    /// </summary>
    public string Season { get; set; }
    
    /// <summary>
    /// The round number in the season (e.g., "1" for the first race, "23" for the 23rd).
    /// </summary>
    public string Round { get; set; }
    
    /// <summary>
    /// Wikipedia URL with more information about this race.
    /// </summary>
    public string Url { get; set; }
    
    /// <summary>
    /// Official name of the race (e.g., "Monaco Grand Prix", "Abu Dhabi Grand Prix").
    /// </summary>
    public string RaceName { get; set; }
    
    /// <summary>
    /// Details about the circuit where this race was held.
    /// Includes circuit ID, name, location, and coordinates.
    /// </summary>
    public Circuit Circuit { get; set; }
    
    /// <summary>
    /// Date when the race took place (e.g., "2024-03-17").
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Race start time in UTC format (e.g., "14:00:00Z").
    /// </summary>
    public string Time { get; set; }
    
    /// <summary>
    /// List of final race results for all drivers who competed in this race.
    /// </summary>
    public List<Result> Results { get; set; }
    
    /// <summary>
    /// List of qualifying session results that determined grid positions for this race.
    /// </summary>
    public List<QualifyingResult> QualifyingResults { get; set; }
    
    /// <summary>
    /// List of pit stop data showing when each driver pitted during the race.
    /// </summary>
    public List<PitStop> PitStops { get; set; }
    
    /// <summary>
    /// Detailed lap-by-lap timing data for drivers during the race.
    /// </summary>
    public List<LapData> Laps { get; set; }
    
    /// <summary>
    /// Sprint race results (when applicable - some seasons include sprint races).
    /// </summary>
    public List<Sprint> SprintResults { get; set; }
}
