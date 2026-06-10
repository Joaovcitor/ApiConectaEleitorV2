using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface ICitizenRepository
{
    Task AddAsync(Citizen citizen);
    Task<Citizen> GetAsync(Guid citizenId, Guid ownerId);
    Task<bool> FindByCpf(string cpf);
    Task<PagedResult<Citizen>> GetPagedAsync(PaginationParams pagination, Guid ownerId);
    Task<PagedResult<Citizen>> GetPagedCitizensLeaderAsync(PaginationParams pagination, Guid ownerId);
    void Update(Citizen citizen);
    void Delete(Citizen citizen);
    Task SaveChangesAsync();
}