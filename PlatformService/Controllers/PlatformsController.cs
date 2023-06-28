using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.MessageBus;
using PlatformService.Models;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(IPlatformRepository repository, 
        IMapper mapper,
        IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var platforms = _repository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpGet("{id}")]
    public ActionResult GetPlatformById(Guid id)
    {
        var platform = _mapper.Map<PlatformReadDto>(_repository.GetPlatformById(id));
        if (platform is null)
        {
            return NotFound();
        }
        return Ok(platform);
    }

    [HttpPost]
    public ActionResult AddPlatform([FromBody] PlatformCreateDto createPlatformDto)
    {
        var platform = _mapper.Map<Platform>(createPlatformDto);
        _repository.CreatePlatform(platform);
        bool result = _repository.SaveChanges();
        if (!result)
        {
            return BadRequest();
        }

        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platform);
            platformPublishedDto.Event = "Platform_Published";
            _messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not send message: {e.Message}.");
        }

        return Ok(platform);
    }
}
