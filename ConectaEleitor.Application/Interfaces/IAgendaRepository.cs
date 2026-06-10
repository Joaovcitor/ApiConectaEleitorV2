using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface IAgendaRepository
{
    Task AddAsync(Agenda agenda);
    Task<Agenda> GetByIdAsync(Guid id, Guid ownerId);
    Task<PagedResult<Agenda>> GetAllAsync(PaginationParams pagination, Guid ownerId); 
    void UpdateAsync(Agenda agenda);
    void DeleteAsync(Agenda agenda);
    Task SaveChangesAsync();
}