using CommandsService.SyncDataServices.Grpc;

namespace CommandService.Data;

public static class SeedData
{
    public static void SeedPlatformsData(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformGrpcClient>();
            if (grpcClient is null)
            {
                return;
            }

            var platforms = grpcClient.ReturnAllPlatforms();

            var repository = serviceScope.ServiceProvider.GetService<ICommandRepository>();
            if (repository == null || platforms == null)
            {
                return;
            }

            foreach (var platform in platforms)
            {
                if (!repository.PlatformExists(platform.Id))
                {
                    repository.CreatePlatform(platform);
                }
            }

            repository.SaveChanges();
        }
    }
}
