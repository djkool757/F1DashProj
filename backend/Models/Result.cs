namespace Pitwall.Models;

/// <summary>
/// Represents the final race result for a single driver in a Grand Prix, including timing and fastest lap data.
/// </summary>
public class Result
{
    /// <summary>
    /// The driver's car number for this race.
    /// </summary>
    public string Number { get; set; }
    
    /// <summary>
    /// The final position where the driver finished (1-20 typically, or higher if DNF).
    /// </summary>
    public string Postion { get; set; }
    
    /// <summary>
    /// String representation of the finishing position (e.g., "1", "2", "+1 Lap", "DNF").
    /// </summary>
    public string PositionText { get; set; }
    
    /// <summary>
    /// Championship points awarded for this finishing position.
    /// </summary>
    public string Points { get; set; }
    
    /// <summary>
    /// The driver who achieved this result with full driver information.
    /// </summary>
    public Driver Driver { get; set; }
    
    /// <summary>
    /// The constructor (team) that the driver was driving for.
    /// </summary>
    public Constructor Constructor { get; set; }
    
    /// <summary>
    /// The grid position where the driver started the race.
    /// </summary>\n    public string Grid { get; set; }
    
    /// <summary>
    /// The total number of laps completed by the driver.
    /// </summary>\n    public string Laps { get; set; }
    
    /// <summary>
    /// The completion status (e.g., \"Finished\", \"Retired\", \"DNF - Engine\").
    /// </summary>\n    public string Status { get; set; }
    
    /// <summary>
    /// The race completion time or gap behind the winner if the driver didn't finish.
    /// Includes total milliseconds and formatted time string.
    /// </summary>\n    public ResultTime Time { get; set; }
    
    /// <summary>
    /// Information about the fastest lap achieved by the driver, if applicable.
    /// Includes the lap number, time, and average speed.
    /// </summary>\n    public FastestLap FastestLap { get; set; }
}

/// <summary>
/// Represents race completion time with both milliseconds (for precise calculation) and human-readable format.
/// </summary>\npublic class ResultTime
//{
    /// <summary>
    /// The total race time in milliseconds for precise calculations.
    /// </summary>\n    public string Millis { get; set; }
    
    /// <summary>
    /// The race time in human-readable format (HH:MM:SS.sss or gap notation).
    /// </summary>\n    public string Time { get; set; }
//}

/// <summary>
/// Represents the fastest lap achieved by a driver, including the lap number, time, and average speed.
/// </summary>\npublic class FastestLap
//{
    /// <summary>
    /// The rank of this fastest lap across all drivers (1 = overall fastest lap, etc.).\n    /// </summary>\n    public string Rank { get; set; }
    
    /// <summary>
    /// The lap number on which this fastest lap was achieved.
    /// </summary>\n    public string Lap { get; set; }
    
    /// <summary>
    /// The duration of the fastest lap.\n    /// </summary>\n    public ResultTime Time { get; set; }
    
    /// <summary>
    /// The average lap speed during the fastest lap.
    /// </summary>\n    public AverageSpeed AverageSpeed { get; set; }
//}

/// <summary>
/// Represents the average speed for a fastest lap, including the unit of measurement.
/// </summary>\npublic class AverageSpeed
//{
    /// <summary>
    /// The unit of speed measurement (typically \"km/h\" or \"mph\").
    /// </summary>\n    public string Units { get; set; }
    
    /// <summary>
    /// The average speed value during the fastest lap.
    /// </summary>\n    public string Speed { get; set; }
//}
