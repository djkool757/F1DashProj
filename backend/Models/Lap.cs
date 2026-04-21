namespace Pitwall.Models;

/// <summary>
/// Represents lap-by-lap timing data for a single lap of a race.
/// Contains the lap number and a list of all drivers' lap times for that lap.
/// </summary>
public class LapData
{
    /// <summary>
    /// The lap number in the race (e.g., "1\" = first lap, \"71\" = 71st lap of the race).
    /// </summary>
    public string Number { get; set; }
    
    /// <summary>
    /// The list of LapTiming objects containing each driver's timing data for this lap.
    /// Includes the driver's position and lap time for this specific lap.
    /// </summary>
    public List<LapTiming> Timings { get; set; }
}

/// <summary>
/// Represents a driver's timing data for a single lap of a race.
/// Contains the driver identifier, position during that lap, and the lap time.
/// </summary>
public class LapTiming
{
    /// <summary>
    /// The unique identifier for the driver (e.g., "hamilton\", \"verstappen\").
    /// </summary>
    public string DriverId { get; set; }
    
    /// <summary>
    /// The driver's position on the track during this lap (1 = leading, 2 = second, etc.).
    /// Note: Property is named "Postion" due to API consistency; represents position during the lap.
    /// </summary>
    public int Postion { get; set; }
    
    /// <summary>
    /// The driver's lap time for this individual lap in format MM:SS.mmm (e.g., "1:34.567\").
    /// Represents the time to complete one circuit of the track.
    /// </summary>
    public string Time { get; set; }
}
