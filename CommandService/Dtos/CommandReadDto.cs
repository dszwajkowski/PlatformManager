namespace CommandService.Dtos;

public class CommandReadDto
{
    public Guid Id { get; set; }
    public string HowTo { get; set; } = null!;
    public string CommandLine { get; set; } = null!;
    public Guid PlatformId { get; set; }
}
