using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using System.Text.Json;

namespace CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory,
        IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }


    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        if (eventType == null)
        {
            return EventType.Undetermined;
        }

        switch (eventType.Event)
        {
            case "Platform_Published":
                AddPlatform(notificationMessage);
                return EventType.PlatformPublished;
            default:
                Console.WriteLine($"Could not determin event type: {eventType.Event}.");
                return EventType.Undetermined;
        };
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using (var scope = _scopeFactory.CreateScope()) 
        {
            var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

            try
            {
                var platform = _mapper.Map<Platform>(platformPublishedDto);

                if (!repository.PlatformExists(platform.Id))
                {
                    repository.CreatePlatform(platform);
                    repository.SaveChanges();
                }
                else
                {
                    Console.WriteLine($"Platform {platform.Name} already exists.");
                };

            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not add platform to db: {e.Message}.");
            }
        }
    }
}
