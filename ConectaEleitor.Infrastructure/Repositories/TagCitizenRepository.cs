using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class TagCitizenRepository : ITagCitizenRepository
{
    private readonly AppDbContext _context;

    public TagCitizenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TagCitizen tagCitizen)
    {
        await _context.TagCitizens.AddAsync(tagCitizen);
    }

    public async Task<TagCitizen?> GetByIdAsync(Guid tagId, Guid citizenId, Guid ownerId)
    {
        return await _context.TagCitizens
            .Include(tc => tc.Tag)
            .Include(tc => tc.Citizen)
            .FirstOrDefaultAsync(tc => 
                tc.TagId == tagId &&
                tc.CitizenId == citizenId &&
                tc.Tag.OwnerId == ownerId &&
                tc.Citizen.OwnerId == ownerId);
    }

    public async Task<IEnumerable<TagCitizen>> GetAllByCitizenIdAsync(Guid citizenId, Guid ownerId)
    {
        return await _context.TagCitizens
            .Include(tc => tc.Tag)
            .Include(tc => tc.Citizen)
            .Where(tc =>
                tc.CitizenId == citizenId &&
                tc.Citizen.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<TagCitizen>> GetAllByTagIdAsync(Guid tagId, Guid ownerId)
    {
        return await _context.TagCitizens
            .Include(tc => tc.Tag)
            .Include(tc => tc.Citizen)
            .Where(tc =>
                tc.TagId == tagId &&
                tc.Tag.OwnerId == ownerId)
            .ToListAsync();
    }

    public void Delete(TagCitizen tagCitizen)
    {
        _context.TagCitizens.Remove(tagCitizen);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}