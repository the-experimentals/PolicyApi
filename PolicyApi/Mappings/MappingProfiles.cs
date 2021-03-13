using System;
using AutoMapper;
using PolicyApi.ResponseModel;

namespace PolicyApi.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TokenResponse, Protos.TokenResponse>()
                .ForMember(dest => dest.ACCESS, opt => opt.MapFrom(src => src.ACCESS));                
        }
    }
}
