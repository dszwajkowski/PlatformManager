using CommandService.Models;

namespace CommandService.Data;

public interface ICommandRepository
{
    bool SaveChanges();

    IEnumerable<Platform> GetPlatforms();
    void CreatePlatform(Platform platform);
    bool PlatformExists(int platformId);

    IEnumerable<Command> GetCommandsForPlatform(Guid platformId);
    Command? GetCommand(Guid platformId, Guid commandId);
    void CreateCommand(Guid platformId, Command command);
}
