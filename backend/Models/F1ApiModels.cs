namespace Pitwall.Models;

/// <summary>
/// Root wrapper for all F1 API responses.
/// The API returns data wrapped in this response object containing metadata and the actual data.
/// </summary>
public class F1ApiResponse
{
    /// <summary>
    /// The main data container from the Ergast F1 API, containing metadata and response data.
    /// </summary>
    public MrData MRData { get; set; }
}

/// <summary>
/// Metadata and data container from the Ergast Motor Racing Data API.
/// "MR" stands for "Motor Racing". This object contains pagination info, format details, and the actual data tables.
/// </summary>
public class MrData
{
    /// <summary>
    /// XML namespace declaration for the API response format.
    /// </summary>
    public string Xmlns { get; set; }
    
    /// <summary>
    /// The series identifier (e.g., "f1" for Formula 1).
    /// </summary>
    public string Series { get; set; }
    
    /// <summary>
    /// The URL of the API endpoint that was called.
    /// </summary>
    public string Url { get; set; }
    
    /// <summary>
    /// The maximum number of records returned in this response (pagination limit).
    /// </summary>
    public string Limit { get; set; }
    
    /// <summary>
    /// The starting position of records in this response (pagination offset).
    /// </summary>
    public string Offset { get; set; }
    
    /// <summary>
    /// The total number of records available for the requested query (not limited by Limit).
    /// </summary>
    public string Total { get; set; }
    
    /// <summary>
    /// Container for race data. Present when querying race-related endpoints.
    /// </summary>
    public RaceTable RaceTable { get; set; }
    
    /// <summary>
    /// Container for circuit/venue data. Present when querying circuit-related endpoints.
    /// </summary>
    public CircuitTable CircuitTable { get; set; }
    
    /// <summary>
    /// Container for driver data. Present when querying driver-related endpoints.
    /// </summary>
    public DriverTable DriverTable { get; set; }
    
    /// <summary>
    /// Container for constructor (team) data. Present when querying constructor-related endpoints.
    /// </summary>
    public ConstructorTable ConstructorTable { get; set; }
    
    /// <summary>
    /// Container for championship standings. Present when querying standings-related endpoints.
    /// </summary>
    public StandingsTable StandingsTable { get; set; }
    
    /// <summary>
    /// Container for status codes used in race results. Present when querying status endpoints.
    /// </summary>
    public StatusTable StatusTable { get; set; }
}

/// <summary>
/// Container for race data from a specific season.
/// </summary>
public class RaceTable
{
    /// <summary>
    /// The F1 season/year (e.g., "2024").
    /// </summary>
    public string Season { get; set; }
    
    /// <summary>
    /// The round number within the season (e.g., "1" for the first race).
    /// </summary>
    public string Round { get; set; }
    
    /// <summary>
    /// List of races in this season or round.
    /// </summary>
    public List<Race> Races { get; set; }
}

/// <summary>
/// Represents a Grand Prix circuit/venue where F1 races are held.
/// Contains information about the track location, name, and Wikipedia URL.
/// </summary>
public class Circuit
{
    /// <summary>
    /// Unique identifier for the circuit (e.g., "bahrain", "monaco", "silverstone").
    /// </summary>
    public string CircuitId { get; set; }
    
    /// <summary>
    /// Display name of the circuit (e.g., "Albert Park", "Circuit of the Americas").
    /// </summary>
    public string CircuitName { get; set; }
    
    /// <summary>
    /// Wikipedia URL for more information about the circuit.
    /// </summary>
    public string Url { get; set; }
    
    /// <summary>
    /// Geographic location details of the circuit (city, country, latitude, longitude).
    /// </summary>
    public Location Location { get; set; }
}

/// <summary>
/// Container for circuit/venue data from the API.
/// </summary>
public class CircuitTable
{
    /// <summary>
    /// List of F1 circuits.
    /// </summary>
    public List<Circuit> Circuits { get; set; }
}

/// <summary>
/// Container for driver data from a specific season.
/// </summary>
public class DriverTable
{
    /// <summary>
    /// The F1 season/year (e.g., "2024").
    /// </summary>
    public string Season { get; set; }
    
    /// <summary>
    /// List of drivers competing in the specified season.
    /// </summary>
    public List<Driver> Drivers { get; set; }
}

/// <summary>
/// Container for constructor (team) data from a specific season.
/// </summary>
public class ConstructorTable
{
    /// <summary>
    /// The F1 season/year (e.g., "2024").
    /// </summary>
    public string Season { get; set; }
    
    /// <summary>
    /// List of constructors (teams) competing in the specified season.
    /// </summary>
    public List<Constructor> Constructors { get; set; }
}

/// <summary>
/// Container for championship standings data.
/// Can contain either driver standings or constructor standings or both.
/// </summary>
public class StandingsTable
{
    /// <summary>
    /// The F1 season/year for which standings are provided (e.g., "2024").
    /// </summary>
    public string Season { get; set; }
    
    /// <summary>
    /// The round number of the season for which standings are current (e.g., "10" for after round 10).
    /// </summary>
    public string Round { get; set; }
    
    /// <summary>
    /// List of driver championship standings snapshots (typically one per round or one for the season).
    /// </summary>
    public List<DriverStandingsList> StandingsLists { get; set; }
    
    /// <summary>
    /// List of constructor championship standings snapshots.
    /// </summary>
    public List<ConstructorStandingsList> ConstructorStandingsLists { get; set; }
}

/// <summary>
/// Container for status codes used in race results.
/// Status codes indicate how a driver finished a race (e.g., "Finished", "DNF - Engine", "Retired").
/// </summary>
public class StatusTable
{
    /// <summary>
    /// List of all possible status codes used in F1 race results.
    /// </summary>
    public List<Status> Status { get; set; }
}
