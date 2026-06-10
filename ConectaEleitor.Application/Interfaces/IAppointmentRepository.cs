using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment);
    Task<Appointment> GetAsync(Guid appointmentId, Guid ownerId);
    Task<PagedResult<Appointment>> GetAllAsync(Guid ownerId, PaginationParams paginationParams);
    void Update(Appointment appointment);
    void Delete(Appointment appointment);
    Task SaveChangesAsync();
}