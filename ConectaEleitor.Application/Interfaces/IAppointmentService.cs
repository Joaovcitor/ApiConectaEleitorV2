using ConectaEleitor.Application.DTOs.AppointmentsDTOs;
using ConectaEleitor.Application.DTOs.Pagination;

namespace ConectaEleitor.Application.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentResponseDTO> Create(AppointmentCreateDTO appointmentCreateDTO);
    Task<AppointmentResponseDTO> GetById(Guid apppointmentId);
    Task<PagedResult<AppointmentResponseDTO>> GetAll(PaginationParams paginationParams);
    Task<AppointmentResponseByIdDTO> GetAppointmentById(Guid appointmentId);
    Task<AppointmentResponseDTO> Update(Guid appointmentId, AppointmentsUpdateDTO appointmentUpdateDTO);
    Task<string> Delete(Guid appointmentId);
}