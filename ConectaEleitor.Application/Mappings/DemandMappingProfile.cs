using AutoMapper;
using ConectaEleitor.Application.DTOs.DemandsDTOs;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Mappings;

public class DemandMappingProfile : Profile
{
    public DemandMappingProfile()
    {
        CreateMap<DemandCreateDTO, Demand>();

        CreateMap<DemandUpdateDTO, Demand>();

        CreateMap<Demand, DemandResponseDTO>()
            .ForMember(dest => dest.CitizenName,
                opt => opt.MapFrom(src => src.Citizen.FullName));

        CreateMap<Demand, DemandResponseByIdDTO>()
            .ForMember(dest => dest.CitizenName,
                opt => opt.MapFrom(src => src.Citizen.FullName));
    }
}