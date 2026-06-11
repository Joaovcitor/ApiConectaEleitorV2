using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface IPlanRepository
{
    Task<Plan?> GetByIdAsync(Guid planId);

    Task<Plan?> GetBySlugAsync(string slug);

    Task<bool> ExistsBySlugAsync(string slug, Guid? ignorePlanId = null);

    Task<bool> HasSubscriptionsAsync(Guid planId);

    Task<PagedResult<Plan>> GetPagedAsync(
        PaginationParams paginationParams,
        bool onlyActive = true);

    Task<PagedResult<Plan>> GetPublicPagedAsync(PaginationParams paginationParams);

    Task AddAsync(Plan plan);

    void Update(Plan plan);

    void Delete(Plan plan);

    Task SaveChangesAsync();
}
