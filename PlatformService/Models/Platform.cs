using System.ComponentModel.DataAnnotations;

namespace PlatformService.Models;

public class Platform
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Publisher { get; set; } = null!;
    [Required]
    public string Cost { get; set; } = null!; 
}
