using System.ComponentModel.DataAnnotations;

namespace PlatformService.Domain;

public class Platform 
{
    [Key]
    [Required]
    public int Id {get;set;}
    public string? Name {get;set;}
    public string? Publisher {get;set;}
    public string? Cost {get;set;}
}