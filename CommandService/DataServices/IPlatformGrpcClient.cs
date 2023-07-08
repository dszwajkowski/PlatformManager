using CommandService.Models;

namespace CommandsService.SyncDataServices.Grpc
{
    public interface IPlatformGrpcClient
    {
        IEnumerable<Platform> ReturnAllPlatforms();
    }
}