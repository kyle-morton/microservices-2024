using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using PlatformService.Repos;

namespace PlatformService.DataServices.Sync.Grpc;

public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
        private readonly IMapper _mapper;
        private readonly IPlatformRepo _repo;

    public GrpcPlatformService(IPlatformRepo repo, IMapper mapper)
    {
            _repo = repo;
            _mapper = mapper;
    }

    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context) 
    {
        var response = new PlatformResponse();
        var platforms = _repo.Get();

        foreach(var plat in platforms) 
        {
            response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
        }

        return Task.FromResult(response);
    }

}
