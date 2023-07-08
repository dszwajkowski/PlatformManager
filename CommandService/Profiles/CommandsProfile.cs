using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;
using PlatformService;

namespace CommandService.Profiles;

public class CommandsProfile : Profile
{
	public CommandsProfile()
	{
		CreateMap<Platform, PlatformReadDto>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>(); 
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformPublishedDto, Platform>();
        CreateMap<GrpcPlatformModel, Platform>()
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PlatformId));
    }
}
