using System.ComponentModel.DataAnnotations;

namespace PlatformService.Models;

public class PlatformCreateModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Publisher { get; set; }
    [Required]
    public string Cost { get; set; }
}