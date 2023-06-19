using CommandService.Models;

namespace CommandService.Data;

public class CommandRepository : ICommandRepository
{
    private readonly AppDbContext _context;

    public CommandRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Platform> GetPlatforms()
    {
        return _context.Platforms.AsEnumerable();
    }

    public void CreatePlatform(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        platform.Id = Guid.NewGuid();
        _context.Platforms.Add(platform);
    }

    public bool PlatformExists(int platformId)
    {
        return _context.Platforms.Any();
    }

    public Command? GetCommand(Guid platformId, Guid commandId)
    {
        return _context.Commands
            .Where(x => x.Id == commandId
                    && x.PlatformId == platformId)
            .FirstOrDefault();
    }

    public void CreateCommand(Guid platformId, Command command)
    {
        ArgumentNullException.ThrowIfNull(command);

        command.Id = Guid.NewGuid();
        command.PlatformId = platformId;

        _context.Commands.Add(command);
    }

    public IEnumerable<Models.Command> GetCommandsForPlatform(Guid platformId)
    {
        return _context.Commands
            .Where(x => x.PlatformId == platformId);
    }


    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}
