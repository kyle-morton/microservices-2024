using System.ComponentModel.DataAnnotations;

namespace CommandsService.Domain;

public class Command
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string HowTo { get; set; }
    [Required]
    public required string CommandLine { get; set; }
    [Required]
    public int PlatformId { get; set; }

    public required Platform Platform { get; set; } 
}