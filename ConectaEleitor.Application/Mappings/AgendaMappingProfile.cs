using AutoMapper;
using ConectaEleitor.Application.DTOs.AgendasDTOs;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Mappings;

public class AgendaMappingProfile : Profile
{
    public AgendaMappingProfile()
    {
        CreateMap<AgendaCreateDTO, Agenda>();

        CreateMap<AgendaUpdateDTO, Agenda>();

        CreateMap<Agenda, AgendaResponseDTO>()
            .ForMember(dest => dest.TotalAppointments,
                opt => opt.MapFrom(src => src.Appointments.Count));

        CreateMap<Agenda, AgendaResponseByIdDTO>();
    }
}