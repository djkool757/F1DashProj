namespace Pitwall.Models;

/// <summary>
/// Represents a constructor (team) championship standing including points, position, and wins.
/// Constructor standings reflect the combined performance of both drivers for a team.
/// </summary>
public class ConstructorStanding
{
    /// <summary>
    /// The constructor's current championship position in the constructor standings (1 = championship leader, 2 = second, etc.).
    /// </summary>
    public string Position { get; set; }
    
    /// <summary>
    /// The text representation of the constructor's championship position (e.g., "1\", \"2\", etc.).
    /// Used for display purposes.
    /// </summary>
    public string PositionText { get; set; }
    
    /// <summary>
    /// The total championship points accumulated by the constructor across all races in the season.
    /// Points combine results from both drivers and are awarded based on finishing positions.
    /// </summary>
    public string Points { get; set; }
    
    /// <summary>
    /// The total number of race wins achieved by the constructor (combined from both drivers) in the season so far.
    /// Used as a tiebreaker in the constructor championship standings.
    /// </summary>
    public int Wins { get; set; }
    
    /// <summary>
    /// The Constructor object containing the team information, name, and country.
    /// </summary>
    public Constructor Constructor { get; set; }
}

/// <summary>
/// Container for constructor standings data from a specific season and round.
/// Aggregates team standings information with metadata about the time period.
/// </summary>
public class ConstructorStandingsTable
{
    /// <summary>
    /// The F1 season year for these standings (e.g., "2024\", \"2023\").
    /// </summary>
    public string Season { get; set; }
    
    /// <summary>
    /// The round/race number these standings represent (e.g., "1\" = after first race, \"22\" = after 22nd race).
    /// </summary>
    public int round { get; set; }
    
    /// <summary>
    /// The list of ConstructorStandingsList objects, typically containing one entry for the current standings.
    /// </summary>
    public List<ConstructorStandingsList> StandingsLists { get; set; }
}

/// <summary>
/// Represents constructor standings for a specific season and round, containing all teams' championship positions.
/// </summary>
public class ConstructorStandingsList
{
    /// <summary>
    /// The F1 season year for these constructor standings (e.g., "2024\", \"2023\").
    /// </summary>
    public string Season { get; set; }
    
    /// <summary>
    /// The round/race number these standings represent (e.g., "1\" = after first race, \"22\" = final standings).
    /// </summary>
    public int round { get; set; }
    
    /// <summary>
    /// The list of all ConstructorStanding objects for this season/round, ranked by championship position.
    /// Includes all constructors competing in the season.
    /// </summary>
    public List<ConstructorStanding> ConstructorStandings { get; set; }
}

/// <summary>
/// Response wrapper for constructor standings data from the Ergast API.
/// </summary>
public class ConstructorStandingsResponse
{
    /// <summary>
    /// The standings table containing constructor positions and point information.
    /// </summary>
    public ConstructorStandingsTable StandingsTable { get; set; }
}
