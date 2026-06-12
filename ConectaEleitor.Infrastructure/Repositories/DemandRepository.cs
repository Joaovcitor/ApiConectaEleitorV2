using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class DemandRepository : IDemandRepository
{
    private readonly AppDbContext _context;

    public DemandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Demand demand)
    {
        await _context.Demands.AddAsync(demand);
    }

    public async Task<Demand> GetByIdAsync(Guid demandId, Guid ownerId)
    {
        return await _context.Demands.Include(x => x.Citizen)
            .FirstOrDefaultAsync(de => de.DemandId == demandId && de.OwnerId == ownerId);
    }

    public async Task<PagedResult<Demand>> GetAllAsync(PaginationParams pagination, Guid ownerId)
    {
        var query = _context.Demands.AsNoTracking()
            .Where(de => de.OwnerId == ownerId)
            .Include(x => x.Citizen)
            .OrderByDescending(de => de.CreatedAt);
        var totalCount = await query.CountAsync();
        var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
        return new PagedResult<Demand>(data, totalCount, pagination.PageNumber, pagination.PageSize);
    }

    public async Task<PagedResult<Demand>> GetAllDemandsByCitizenId(PaginationParams pagination, Guid citizenId, Guid ownerId)
    {
        var query =  _context.Demands.AsNoTracking()
            .Where(d => d.CitizenId == citizenId && d.OwnerId == ownerId)
            .OrderByDescending(d => d.CreatedAt);
        var totalCount = await query.CountAsync();
        var data = await  query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
        return new PagedResult<Demand>(data, totalCount, pagination.PageNumber, pagination.PageSize);
    }

    public void Update(Demand demand)
    {
        _context.Demands.Update(demand);
    }

    public void Delete(Demand demand)
    {
        _context.Demands.Remove(demand);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}