namespace PlatformService.Models;

public class PlatformPublishedModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public required string Event { get; set; }
}