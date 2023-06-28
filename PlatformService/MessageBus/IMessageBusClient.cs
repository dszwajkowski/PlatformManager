using PlatformService.Dtos;

namespace PlatformService.MessageBus;

public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
}
