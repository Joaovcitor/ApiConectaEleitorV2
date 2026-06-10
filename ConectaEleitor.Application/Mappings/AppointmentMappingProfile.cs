using AutoMapper;
using ConectaEleitor.Application.DTOs.AppointmentsDTOs;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Mappings;

public class AppointmentMappingProfile : Profile
{
    public AppointmentMappingProfile()
    {
        CreateMap<AppointmentCreateDTO, Appointment>()
            .ForMember(
                dest => dest.StartAt,
                opt => opt.MapFrom(src =>
                    DateTime.SpecifyKind(src.StartAt, DateTimeKind.Utc)))
            .ForMember(
                dest => dest.EndAt,
                opt => opt.MapFrom(src =>
                    DateTime.SpecifyKind(src.EndAt, DateTimeKind.Utc)));

        CreateMap<AppointmentsUpdateDTO, Appointment>()
            .ForMember(
                dest => dest.StartAt,
                opt => opt.MapFrom(src =>
                    DateTime.SpecifyKind(src.StartAt, DateTimeKind.Utc)))
            .ForMember(
                dest => dest.EndAt,
                opt => opt.MapFrom(src =>
                    DateTime.SpecifyKind(src.EndAt, DateTimeKind.Utc)));

        CreateMap<Appointment, AppointmentResponseDTO>()
            .ForMember(dest => dest.AgendaName,
                opt => opt.MapFrom(src => src.Agenda.Name))
            .ForMember(dest => dest.CitizenName,
                opt => opt.MapFrom(src => src.Citizen != null ? src.Citizen.FullName : null));

        CreateMap<Appointment, AppointmentResponseByIdDTO>()
            .ForMember(dest => dest.AgendaName,
                opt => opt.MapFrom(src => src.Agenda.Name))
            .ForMember(dest => dest.CitizenName,
                opt => opt.MapFrom(src => src.Citizen != null ? src.Citizen.FullName : null));
    }
}