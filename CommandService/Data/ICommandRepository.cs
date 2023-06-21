using CommandService.Models;

namespace CommandService.Data;

public interface ICommandRepository
{
    bool SaveChanges();

    IEnumerable<Platform> GetPlatforms();
    Guid CreatePlatform(Platform platform);
    bool PlatformExists(Guid platformId);

    IEnumerable<Command> GetCommandsForPlatform(Guid platformId);
    Command? GetCommand(Guid platformId, Guid commandId);
    Guid CreateCommand(Guid platformId, Command command);
}
