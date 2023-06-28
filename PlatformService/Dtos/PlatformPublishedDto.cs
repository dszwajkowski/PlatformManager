namespace PlatformService.Dtos;

public class PlatformPublishedDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Event { get; set; } = null!;
}
