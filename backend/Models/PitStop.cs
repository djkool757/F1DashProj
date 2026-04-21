namespace Pitwall.Models;

/// <summary>
/// Represents a pit stop event during a race where a driver stopped at the pit lane for tire change or repairs.
/// Contains stop number, lap number, and timing information.
/// </summary>
public class PitStop
{
    /// <summary>
    /// The unique identifier for the driver who made the pit stop (e.g., "hamilton\", \"verstappen\").
    /// </summary>
    public string DriverId { get; set; }
    
    /// <summary>
    /// The lap number on which the pit stop occurred (e.g., "23\", \"45\").
    /// </summary>
    public string Lap { get; set; }
    
    /// <summary>
    /// The pit stop number for this driver during the race (e.g., "1\", \"2\", \"3\").
    /// Drivers typically make 1-3 stops per race depending on weather and strategy.
    /// </summary>
    public string Stop { get; set; }
    
    /// <summary>
    /// The time on the race clock when the pit stop completed in format HH:MM:SS.mmm (e.g., "1:23:45.678\").
    /// </summary>
    public string Time { get; set; }
    
    /// <summary>
    /// The duration of the pit stop in format MM:SS.mmm (e.g., "23.456\" for 23.456 seconds).
    /// Duration includes tire change time and any repairs.
    /// </summary>
    public string Duration { get; set; }
}
