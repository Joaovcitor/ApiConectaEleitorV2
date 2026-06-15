using AutoMapper;
using ConectaEleitor.Application.DTOs.TagCitizensDTOs;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Mappings;

public class TagCitizenProfile : Profile
{
    public TagCitizenProfile()
    {
        CreateMap<TagCitizenCreateDTO, TagCitizen>();

        CreateMap<TagCitizenUpdateDTO, TagCitizen>()
            .ForMember(dest => dest.TagId, opt => opt.Ignore())
            .ForMember(dest => dest.CitizenId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Tag, opt => opt.Ignore())
            .ForMember(dest => dest.Citizen, opt => opt.Ignore());

        CreateMap<TagCitizen, TagCitizenResponseDTO>()
            .ForMember(dest => dest.TagName,
                opt => opt.MapFrom(src => src.Tag.Name))

            .ForMember(dest => dest.TagDescription,
                opt => opt.MapFrom(src => src.Tag.Description))

            .ForMember(dest => dest.TagColor,
                opt => opt.MapFrom(src => src.Tag.Color))

            .ForMember(dest => dest.CitizenName,
                opt => opt.MapFrom(src => src.Citizen.FullName));
    }
}