using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models;

public class CommandReadModel
{
    public int Id { get; set; }
    [Required]
    public required string HowTo { get; set; }
    [Required]
    public required string CommandLine { get; set; }
    public int PlatformId { get; set; }
}