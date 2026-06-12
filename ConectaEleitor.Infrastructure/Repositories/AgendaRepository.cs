using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class AgendaRepository : IAgendaRepository
{
    private readonly AppDbContext _context;

    public AgendaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Agenda agenda)
    {
        await _context.Agendas.AddAsync(agenda);
    }

    public async Task<Agenda> GetByIdAsync(Guid id, Guid ownerId)
    {
        return await _context.Agendas.FirstOrDefaultAsync(a => a.AgendaId == id && a.OwnerId == ownerId);
    }

    public async Task<PagedResult<Agenda>> GetAllAsync(PaginationParams pagination, Guid ownerId)
    {
        var query = _context.Agendas.AsNoTracking()
            .Where(a => a.OwnerId == ownerId)
            .Include(x => x.Appointments)
            .OrderByDescending(a => a.CreatedAt);
        var totalItems = await query.CountAsync();
        var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
        return new PagedResult<Agenda>(data, totalItems, pagination.PageNumber, pagination.PageSize);
    }

    public void UpdateAsync(Agenda agenda)
    {
        _context.Agendas.Update(agenda);
    }

    public void DeleteAsync(Agenda agenda)
    {
        _context.Agendas.Remove(agenda);
    }

    public async Task SaveChangesAsync()
    {
        await  _context.SaveChangesAsync();
    }
}