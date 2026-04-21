namespace Pitwall.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found (404)
/// </summary>
public class ResourceNotFoundException : Exception
{
    public string ResourceType { get; }
    public string ResourceId { get; }

    public ResourceNotFoundException(string resourceType, string resourceId, string? message = null)
        : base(message ?? $"The requested {resourceType} '{resourceId}' was not found.")
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
    }
}
