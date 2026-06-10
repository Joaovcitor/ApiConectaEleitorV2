using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Enums;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class CitizenRepository : ICitizenRepository
{
    private readonly AppDbContext _context;

    public CitizenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Citizen citizen)
    {
        await  _context.Citizens.AddAsync(citizen);
    }

    public async Task<Citizen> GetAsync(Guid citizenId, Guid ownerId)
    {
        return await _context.Citizens.FirstOrDefaultAsync(ci => ci.CitizenId == citizenId && ci.OwnerId == ownerId);
    }

    public async Task<bool> FindByCpf(string cpf)
    {
        return await _context.Citizens.AnyAsync(ci => ci.Cpf == cpf);
    }

    public async Task<PagedResult<Citizen>> GetPagedAsync(PaginationParams pagination, Guid ownerId)
    {
        var query = _context.Citizens.AsNoTracking()
            .Where(ci => ci.OwnerId == ownerId)
            .OrderByDescending(ci => ci.FullName);
        var totalCount = await query.CountAsync();
        var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
        return new PagedResult<Citizen>(data, totalCount, pagination.PageNumber, pagination.PageSize);
    }

    public async Task<PagedResult<Citizen>> GetPagedCitizensLeaderAsync(PaginationParams pagination, Guid ownerId)
    {
        var query = _context.Citizens.AsNoTracking()
            .Where(ci => ci.OwnerId == ownerId && ci.Type == CitizenType.Leader)
            .OrderByDescending(ci => ci.FullName);
        var totalCount = await query.CountAsync();
        var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
        return new PagedResult<Citizen>(data, totalCount, pagination.PageNumber, pagination.PageSize);
    }

    public void Update(Citizen citizen)
    {
        _context.Citizens.Update(citizen);
    }

    public void Delete(Citizen citizen)
    {
        _context.Citizens.Remove(citizen);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}