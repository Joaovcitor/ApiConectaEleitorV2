using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface ITagCitizenRepository
{
    Task AddAsync(TagCitizen tagCitizen);

    Task<TagCitizen?> GetByIdAsync(Guid tagId, Guid citizenId, Guid ownerId);

    Task<IEnumerable<TagCitizen>> GetAllByCitizenIdAsync(Guid citizenId, Guid ownerId);

    Task<IEnumerable<TagCitizen>> GetAllByTagIdAsync(Guid tagId, Guid ownerId);

    void Delete(TagCitizen tagCitizen);

    Task SaveChangesAsync();
}