using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class PostCommentRepository : IPostCommentRepository
{
    private readonly AppDbContext _context;

    public PostCommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AssemblymanPostComment comment)
    {
        await _context.AddAsync(comment);
    }

    public async Task<PagedResult<AssemblymanPostComment>> GetCommentsByPostId(Guid postId, PaginationParams paginationParams)
    {
        var query = _context.AssemblymanPostComments.AsNoTracking()
            .Where(pc => pc.PostId == postId)
            .OrderByDescending(pc => pc.CreatedAt);
        var totalCount = await query.CountAsync();
        var data = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return new PagedResult<AssemblymanPostComment>(data, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<AssemblymanPostComment> GetCommentById(Guid id)
    {
        return await _context.AssemblymanPostComments.FirstOrDefaultAsync(pc => pc.AssemblymanPostCommentId == id);
    }

    public void Update(AssemblymanPostComment comment)
    {
        _context.AssemblymanPostComments.Update(comment);
    }

    public void Delete(AssemblymanPostComment comment)
    {
        _context.AssemblymanPostComments.Remove(comment);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}