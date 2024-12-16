using AutoMapper;
using CommandsService.Domain;
using CommandsService.Models;
// using PlatformService;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadModel>();
            CreateMap<CommandCreateModel, Command>();
            CreateMap<Command, CommandReadModel>();
            CreateMap<PlatformPublishedModel, Platform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            // CreateMap<GrpcPlatformModel, Platform>()
            //     .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.PlatformId))
            //     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //     .ForMember(dest => dest.Commands, opt => opt.Ignore());
        }
    }
}