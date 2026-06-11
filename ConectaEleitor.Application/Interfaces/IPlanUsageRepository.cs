using ConectaEleitor.Domain.Entities;

namespace ConectaEleitor.Application.Interfaces;

public interface IPlanUsageRepository
{
    Task<PlanUsage?> GetByOwnerAndPeriodAsync(Guid ownerId, int year, int month);

    Task AddAsync(PlanUsage usage);

    void Update(PlanUsage usage);

    Task SaveChangesAsync();
}
