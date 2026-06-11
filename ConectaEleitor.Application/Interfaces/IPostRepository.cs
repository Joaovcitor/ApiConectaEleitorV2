using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface IPostRepository
{
    Task AddAsync(AssemblymanPost assemblymanPost);
    Task<AssemblymanPost> FindByIdAsync(Guid assemblymanPostId);
    Task<PagedResult<AssemblymanPost>> GetAllPagedAsync(PaginationParams pagination);
    Task<PagedResult<AssemblymanPost>> GetPostsUserLogged(PaginationParams pagination, Guid userId);
    void Update(AssemblymanPost assemblymanPost);
    void Delete(AssemblymanPost assemblymanPost);
    Task SaveChangesAsync();
}