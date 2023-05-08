using PlatformService.Models;

namespace PlatformService.Data;

public static class AppDbContextSeed
{
    public static void SeedData(AppDbContext context)
    {
        if (!context.Platforms.Any())
        {
            context.AddRange(
                new Platform { Name = "DotNet", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );

            context.SaveChanges();
        }
    }
}
