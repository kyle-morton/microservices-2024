using AutoMapper;
using PlatformService.Domain;
using PlatformService.Models;

namespace PlatformService.Mappings;

public class PlatformsProfile : Profile 
{
    public PlatformsProfile()
    {
        // Source -> Target
        CreateMap<Platform, PlatformReadModel>();
        CreateMap<PlatformCreateModel, Platform>();
        CreateMap<PlatformReadModel, PlatformPublishedModel>();
        CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
    }
}