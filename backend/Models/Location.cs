namespace Pitwall.Models;

/// <summary>
/// Represents the geographic location of an F1 circuit.
/// Contains city, country, and GPS coordinates for circuit identification and mapping.
/// </summary>
public class Location
{
    /// <summary>
    /// Latitude coordinate of the circuit (decimal format, e.g., "-37.8497").
    /// </summary>
    public string Latitude { get; set; }
    
    /// <summary>
    /// Longitude coordinate of the circuit (decimal format, e.g., "144.968").
    /// </summary>
    public string Longitude { get; set; }
    
    /// <summary>
    /// The city or locality where the circuit is located (e.g., "Monaco\", \"Melbourne\", \"New York\").
    /// </summary>
    public string Locality { get; set; }
    
    /// <summary>
    /// The country where the circuit is located (e.g., "Monaco\", \"Australia\", \"USA\").
    /// </summary>
    public string Country { get; set; }
}
