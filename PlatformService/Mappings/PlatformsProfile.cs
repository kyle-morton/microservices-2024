using AutoMapper;
using PlatformService.Domain;
using PlatformService.Models;

namespace PlatformService.Mappings;

public class PlatformsProfile : Profile 
{
    public PlatformsProfile()
    {
        // Source -> Target
        CreateMap<Platform, PlatformRead>();
        CreateMap<PlatformCreate, Platform>();
    }
}