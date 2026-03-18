namespace WebApplication1.Models;

/// <summary>
/// Represents a driver's qualifying session result, including fastest lap times from each qualifying segment.
/// Qualifying determines the grid positions for the race start.
/// </summary>
public class QualifyingResult
{
    /// <summary>
    /// The driver's car number for this qualifying session.
    /// </summary>
    public string Number { get; set; }
    
    /// <summary>
    /// The driver's grid position result from qualifying (1 = pole position, 2 = second on grid, etc.).
    /// Note: Property is named "Postion" due to API consistency; represents grid position.
    /// </summary>
    public int Postion { get; set; }
    
    /// <summary>
    /// The Driver object containing the driver's personal information and identifiers.
    /// </summary>
    public Driver Driver { get; set; }
    
    /// <summary>
    /// The Constructor object containing the team/constructor information.
    /// </summary>
    public Constructor Constructor { get; set; }
    
    /// <summary>
    /// The fastest lap time in Qualifying 1 (Q1) segment in format MM:SS.mmm (e.g., "1:24.567\").
    /// Null if driver was eliminated in Q1 or did not participate.
    /// </summary>
    public string Q1 { get; set; }
    
    /// <summary>
    /// The fastest lap time in Qualifying 2 (Q2) segment in format MM:SS.mmm (e.g., "1:23.456\").
    /// Null if driver was eliminated in Q2 or did not participate beyond Q1.
    /// </summary>
    public string Q2 { get; set; }
    
    /// <summary>
    /// The fastest lap time in Qualifying 3 (Q3) segment in format MM:SS.mmm (e.g., "1:22.345\").
    /// Only the top 10 drivers from Q2 advance to Q3.
    /// Null if driver did not advance to Q3.
    /// </summary>
    public string Q3 { get; set; }
}
