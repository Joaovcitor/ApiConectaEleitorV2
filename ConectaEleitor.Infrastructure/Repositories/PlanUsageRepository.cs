using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class PlanUsageRepository : IPlanUsageRepository
{
    private readonly AppDbContext _context;

    public PlanUsageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PlanUsage?> GetByOwnerAndPeriodAsync(Guid ownerId, int year, int month)
    {
        return await _context.PlanUsages
            .FirstOrDefaultAsync(x => x.OwnerId == ownerId && x.Year == year && x.Month == month);
    }

    public async Task AddAsync(PlanUsage usage)
    {
        await _context.PlanUsages.AddAsync(usage);
    }

    public void Update(PlanUsage usage)
    {
        _context.PlanUsages.Update(usage);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
