using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface IPostCommentRepository
{
    Task AddAsync(AssemblymanPostComment comment);
    Task<PagedResult<AssemblymanPostComment>> GetCommentsByPostId(Guid postId, PaginationParams paginationParams);
    Task<AssemblymanPostComment> GetCommentById(Guid id);
    void Update(AssemblymanPostComment comment);
    void Delete(AssemblymanPostComment comment);
    Task SaveChanges();
}