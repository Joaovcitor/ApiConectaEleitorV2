using AutoMapper;
using ConectaEleitor.Application.DTOs.TagsDTOs;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Mappings;

public class TagProfileMapping : Profile
{
    public TagProfileMapping()
    {
        CreateMap<Tag, TagResponseDTO>();

        CreateMap<TagCreateDTO, Tag>();

        CreateMap<TagUpdateDTO, Tag>()
            .ForMember(dest => dest.TagId, opt => opt.Ignore())
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.TagsCitizens, opt => opt.Ignore());
    }
}