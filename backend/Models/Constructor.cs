namespace Pitwall.Models;

/// <summary>
/// Represents an F1 constructor (team) that builds and races cars in the championship.
/// </summary>
public class Constructor
{
    /// <summary>
    /// Unique identifier for the constructor/team (e.g., "mercedes", "ferrari", "red_bull").
    /// </summary>
    public string ConstructorId { get; set; }
    
    /// <summary>
    /// Wikipedia URL for the constructor with historical information and achievements.
    /// </summary>
    public string Url { get; set; }
    
    /// <summary>
    /// Official name of the constructor/team (e.g., "Mercedes-AMG Petronas", "Scuderia Ferrari").
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Country of origin or registration for the constructor.
    /// </summary>
    public string Nationality { get; set; }
}
