using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface ITagRepository
{
    Task AddAsync(Tag tag);
    Task<PagedResult<Tag>> GetAllAsync(PaginationParams paginationParams, Guid ownerId);
    Task<Tag> GetByIdAsync(Guid id, Guid ownerId);
    Task<Tag> FindByNameAsync(string name, Guid ownerId);
    void Update(Tag tag);
    void Remove(Tag tag);
    Task SaveChangesAsync();
}