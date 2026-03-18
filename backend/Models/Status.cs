namespace WebApplication1.Models;

/// <summary>
/// Represents a race finishing status and how many drivers achieved that status.
/// Status codes indicate why a driver did not finish (e.g., "Retired\", \"Accident\") or special circumstances.
/// </summary>
public class Status
{
    /// <summary>
    /// The unique identifier for the status type (e.g., "1\" for finished, \"3\" for accident, \"107\" for lapped).
    /// </summary>
    public string StatusId { get; set; }
    
    /// <summary>
    /// The number of drivers who had this specific status result in the race.
    /// Indicates distribution of finishing statuses across all drivers.
    /// </summary>
    public string Count { get; set; }
    
    /// <summary>
    /// The human-readable description of the status (e.g., "Finished\", \"Accident\", \"Retired\", \"Lapped\", \"Disqualified\").
    /// Displayed to end users for understanding why drivers did not finish.
    /// </summary>
    public string StatusText { get; set; }
}

/*public class StatusTable
{
    public List<Status> StatusList { get; set; }
}

public class StatusResponse
{
    public StatusTable StatusTable { get; set; }
}
**/
