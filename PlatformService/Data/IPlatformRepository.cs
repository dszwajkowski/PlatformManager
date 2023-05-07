using PlatformService.Models;

namespace PlatformService.Data;

public interface IPlatformRepository
{
    IEnumerable<Platform> GetAllPlatforms();
    Platform? GetPlatformById(Guid id);
    void CreatePlatform(Platform platform); 
    bool SaveChanges();
}
