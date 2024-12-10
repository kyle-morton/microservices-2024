using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models;

public class PlatformReadModel
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }

}