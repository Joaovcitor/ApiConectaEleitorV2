using AutoMapper;
using ConectaEleitor.Application.DTOs.CitizenDTOs;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Mappings;

public class CitizenMappingProfile : Profile
{
    public CitizenMappingProfile()
    {
        CreateMap<CitizenCreateDTO, Citizen>();

        CreateMap<CitizenUpdateDTO, Citizen>();

        CreateMap<Citizen, CitizenResponseDTO>()
            .ForMember(dest => dest.LeaderName,
                opt => opt.MapFrom(src => src.Leader != null ? src.Leader.FullName : null));

        CreateMap<Citizen, CitizenResponseByIdDTO>()
            .ForMember(dest => dest.LeaderName,
                opt => opt.MapFrom(src => src.Leader != null ? src.Leader.FullName : null));
    }
}