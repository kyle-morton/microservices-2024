using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models;

public class CommandCreateModel
{
    [Required]
    public required string HowTo { get; set; }
    [Required]
    public required string CommandLine { get; set; }
}