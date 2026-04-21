namespace Pitwall.Models;

/// <summary>
/// Represents an F1 driver with their personal information and identifiers.
/// </summary>
public class Driver
{
    /// <summary>
    /// Unique identifier for the driver (e.g., "lewis_hamilton", "max_verstappen").
    /// </summary>
    public string DriverId { get; set; }
    
    /// <summary>
    /// The driver's permanent number (e.g., "1" for World Champion, "44" for Lewis Hamilton).
    /// Permanent numbers are assigned to drivers and retained across seasons.
    /// </summary>
    public string PermanentNumber { get; set; }
    
    /// <summary>
    /// Three-letter driver code (e.g., "HAM" for Hamilton, "VER" for Verstappen).
    /// Used for identification on race broadcasts and graphics.
    /// </summary>
    public string Code { get; set; }
    
    /// <summary>
    /// Wikipedia URL for the driver's biography and career statistics.
    /// </summary>
    public string Url { get; set; }
    
    /// <summary>
    /// Driver's given name (first name).
    /// </summary>
    public string GivenName { get; set; }
    
    /// <summary>
    /// Driver's family name (surname).
    /// </summary>
    public string FamilyName { get; set; }
    
    /// <summary>
    /// Driver's date of birth in ISO format (YYYY-MM-DD).
    /// </summary>
    public string DateOfBirth { get; set; }
    
    /// <summary>
    /// Driver's nationality (country flag code, e.g., "British", "Dutch", "German").
    /// </summary>
    public string Nationality { get; set; }
}
