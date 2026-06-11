using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AssemblymanPost assemblymanPost)
    {
        await _context.AddAsync(assemblymanPost);
    }

    public async Task<AssemblymanPost> FindByIdAsync(Guid assemblymanPostId)
    {
        return await _context.AssemblymanPosts.FirstOrDefaultAsync(p => p.AssemblymanPostId == assemblymanPostId);
    }

    public async Task<PagedResult<AssemblymanPost>> GetAllPagedAsync(PaginationParams pagination)
    {
        var query = _context.AssemblymanPosts.AsNoTracking()
            .OrderByDescending(p => p.CreatedAt);
        var totalCount = await query.CountAsync();
        var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
        return new PagedResult<AssemblymanPost>(data, totalCount, pagination.PageNumber, pagination.PageSize);
    }

    public async Task<PagedResult<AssemblymanPost>> GetPostsUserLogged(PaginationParams pagination, Guid userId)
    {
        var query = _context.AssemblymanPosts.AsNoTracking()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt);
        var totalCount = await query.CountAsync();
        var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
        return new PagedResult<AssemblymanPost>(data, totalCount, pagination.PageNumber, pagination.PageSize);
    }

    public void Update(AssemblymanPost assemblymanPost)
    {
       _context.AssemblymanPosts.Update(assemblymanPost);
    }

    public void Delete(AssemblymanPost assemblymanPost)
    {
        _context.AssemblymanPosts.Remove(assemblymanPost);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}