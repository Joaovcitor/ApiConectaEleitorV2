using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppDbContext _context;

    public TagRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Tag tag)
    {
        await _context.Tags.AddAsync(tag);
    }

    public async Task<PagedResult<Tag>> GetAllAsync(PaginationParams paginationParams, Guid ownerId)
    {
        var query = _context.Tags.AsNoTracking()
            .Where(x => x.OwnerId == ownerId)
            .OrderByDescending(x => x.Name);
        var totalCount = await query.CountAsync();
        var data = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return new PagedResult<Tag>(data, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<Tag> GetByIdAsync(Guid id, Guid ownerId)
    {
        return await _context.Tags.FirstOrDefaultAsync(t => t.TagId == id && t.OwnerId == ownerId);
    }

    public async Task<Tag> FindByNameAsync(string name, Guid ownerId)
    {
        return await _context.Tags.FirstOrDefaultAsync(t => t.Name == name && t.OwnerId == ownerId);
    }

    public void Update(Tag tag)
    {
        _context.Tags.Update(tag);
    }

    public void Remove(Tag tag)
    {
        _context.Tags.Remove(tag);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}