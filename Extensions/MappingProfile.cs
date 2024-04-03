using AccessService.Models;
using AutoMapper;

namespace AccessService.Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApiTokenDetails, UserApiTokenView>()
            .ForMember(dest => dest.LastUsage,
                opt => opt.MapFrom(src => src.LastUsage.ToString("yyyy-MM-dd HH:mm:ss")))
            .ForMember(dest => dest.Token,
                opt => opt.MapFrom(src => src.Token.ToString()))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Permissions,
                opt => opt.MapFrom(src => src.Permissions.Select(s => s.ToString())));
    }
}