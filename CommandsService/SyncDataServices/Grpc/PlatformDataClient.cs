using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommandsService.Domain;
using Grpc.Net.Client;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc;


public interface IPlatformDataClient
{
    IEnumerable<Platform> ReturnAllPlatforms();
}

public class PlatformDataClient : IPlatformDataClient
{

    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public PlatformDataClient(IConfiguration config, IMapper mapper)
    {
        _config = config;
        _mapper = mapper;
    }

    public IEnumerable<Platform> ReturnAllPlatforms()
    {
        var grpcPlatform = _config["GrpcPlatform"];
        Console.WriteLine($"PlatformDataClient: Calling Grpc Service {grpcPlatform}");

        var channel = GrpcChannel.ForAddress(grpcPlatform);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);

        var request = new GetAllRequest();
        var platformsToReturn = new List<Platform>();

        try {
            var reply = client.GetAllPlatforms(request);
            platformsToReturn = _mapper.Map<List<Platform>>(reply.Platform);          
        }catch(Exception ex) {
            Console.WriteLine($"PlatformDataClient: Could not call grpc server {ex.Message}");
        }

        return platformsToReturn;
    }
}