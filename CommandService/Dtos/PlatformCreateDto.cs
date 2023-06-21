using System.ComponentModel.DataAnnotations;

namespace CommandService.Dtos;

public class PlatformCreateDto
{
    [Required]
    public string Name { get; set; } = null!;
}
