using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface IDemandRepository
{
    Task AddAsync(Demand demand);
    Task<Demand> GetByIdAsync(Guid demandId, Guid ownerId);
    Task<PagedResult<Demand>> GetAllAsync(PaginationParams pagination, Guid ownerId);
    Task<PagedResult<Demand>> GetAllDemandsByCitizenId(PaginationParams pagination, Guid citizenId, Guid ownerId);
    void  Update(Demand demand);
    void Delete(Demand demand);
    Task SaveChangesAsync();
}