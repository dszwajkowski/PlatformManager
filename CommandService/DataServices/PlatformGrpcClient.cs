using AutoMapper;
using CommandService.Models;
using CommandsService.SyncDataServices.Grpc;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.DataServices;

public class PlatformGrpcClient : IPlatformGrpcClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public PlatformGrpcClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    public IEnumerable<Platform>? ReturnAllPlatforms()
    {
        ArgumentNullException.ThrowIfNullOrEmpty("GrpcPlatform config");
        var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]!);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllPlatforms(request);
            return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not call Grpc Server: {ex.Message}");
            return null;
        }
    }
}
