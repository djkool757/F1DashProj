namespace WebApplication1.Models;

/// <summary>
/// Represents a driver's current championship standing including points, position, and wins.
/// Standings are updated after each race and calculated across the entire season.
/// </summary>
public class DriverStanding
{
    /// <summary>
    /// The driver's current championship position in the driver standings (1 = championship leader, 2 = second, etc.).
    /// Note: Property is named "Postion" due to API consistency; represents championship position.
    /// </summary>
    public string Position { get; set; }
    
    /// <summary>
    /// The text representation of the driver's championship position (e.g., "1\", \"2\", etc.).
    /// Used for display purposes.
    /// </summary>
    public string PositionText { get; set; }
    
    /// <summary>
    /// The total championship points accumulated by the driver across all races in the season.
    /// Points are awarded based on finishing position and fastest lap achievements.
    /// </summary>
    public string Points { get; set; }
    
    /// <summary>
    /// The total number of race wins achieved by the driver in the season so far.
    /// Used as a tiebreaker in the championship standings.
    /// </summary>
    public string Wins { get; set; }
    
    /// <summary>
    /// The Driver object containing the driver's personal information, name, and identifiers.
    /// </summary>
    public Driver Driver { get; set; }
    
    /// <summary>
    /// The list of Constructor objects representing the teams/constructors the driver has driven for in the season.
    /// Most drivers drive for a single constructor; list accommodates mid-season team changes.
    /// </summary>
    public List<Constructor> Constructors { get; set; }
}

/// <summary>
/// Container for driver standings data from a specific season and round.
/// Aggregates standings information with metadata about the time period.
/// </summary>
public class DriverStandingsTable
{
    /// <summary>
    /// The F1 season year for these standings (e.g., "2024\", \"2023\").
    /// </summary>
    public string Season { get; set; }
    
    /// <summary>
    /// The round/race number these standings represent (e.g., "1\" = after first race, \"22\" = after 22nd race).
    /// </summary>
    public string round { get; set; }
    
    /// <summary>
    /// The list of DriverStandingsList objects, typically containing one entry for the current standings.
    /// </summary>
    public List<DriverStandingsList> StandingsLists { get; set; }
}

/// <summary>
/// Represents driver standings for a specific season and round, containing all drivers' championship positions.
/// </summary>
public class DriverStandingsList
{
    /// <summary>
    /// The F1 season year for these driver standings (e.g., "2024\", \"2023\").
    /// </summary>
    public string Season { get; set; }
    
    /// <summary>
    /// The round/race number these standings represent (e.g., "1\" = after first race, \"22\" = final standings).
    /// </summary>
    public string round { get; set; }
    
    /// <summary>
    /// The list of all DriverStanding objects for this season/round, ranked by championship position.
    /// Includes all drivers who have competed and earned championship points.
    /// </summary>
    public List<DriverStanding> DriverStandings { get; set; }
}

/// <summary>
/// Response wrapper for driver standings data from the Ergast API.
/// </summary>
public class DriverStandingsResponse
{
    /// <summary>
    /// The standings table containing driver positions and point information.
    /// </summary>
    public DriverStandingsTable StandingsTable { get; set; }
}
