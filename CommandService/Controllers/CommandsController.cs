using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("/api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : Controller
{
    private readonly ICommandRepository _commandRepository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepository commandRepository, IMapper mapper)
    {
        _commandRepository = commandRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(Guid platformId)
    {
        if (!_commandRepository.PlatformExists(platformId)) 
        {
            return NotFound();
        }

        var commands = _commandRepository.GetCommandsForPlatform(platformId);

        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(Guid platformId, 
        Guid commandId)
    {
        if (!_commandRepository.PlatformExists(platformId))
        {
            return NotFound();
        }

        var command = _commandRepository.GetCommand(platformId, commandId);
        if (command == null) 
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CommandReadDto>(command));
    }

    [HttpPost]
    public ActionResult CraateCommandForPlatform(Guid platformId, CommandCreateDto command)
    {
        if (!_commandRepository.PlatformExists(platformId))
        {
            return NotFound();
        }

        var commandId = _commandRepository.CreateCommand(platformId, _mapper.Map<Command>(command));
        _commandRepository.SaveChanges();

        return Ok(commandId);
    }
}
