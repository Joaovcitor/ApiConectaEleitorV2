using AutoMapper;
using ConectaEleitor.Application.DTOs.PoliticalProfiles;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Mappings;

public class PoliticalProfileMappingProfile : Profile
{
    public PoliticalProfileMappingProfile()
    {
        CreateMap<PoliticalProfileCreateDTO, PoliticalProfile>();

        CreateMap<PoliticalProfileUpdateDTO, PoliticalProfile>()
            .ForMember(dest => dest.PoliticalProfileId, opt => opt.Ignore())
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<PoliticalProfile, PoliticalProfileResponseDTO>();
    }
}