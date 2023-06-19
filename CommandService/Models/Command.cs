using System.ComponentModel.DataAnnotations;

namespace CommandService.Models;

public class Command
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string HowTo { get; set; } = null!;
    [Required]
    public string CommandLine { get; set; } = null!;
    [Required]
    public Guid PlatformId { get; set; }
    public Platform Platform { get; set; } = null!;
}
