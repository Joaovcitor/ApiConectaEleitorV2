using ConectaEleitor.Application.DTOs.Pagination;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Appointment appointment)
    {
        await  _context.Appointments.AddAsync(appointment);
    }

    public async Task<Appointment> GetAsync(Guid appointmentId, Guid ownerId)
    {
        return await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.OwnerId == ownerId);
    }

    public async Task<PagedResult<Appointment>> GetAllAsync(Guid ownerId, PaginationParams paginationParams)
    {
        var query = _context.Appointments.AsNoTracking()
            .Where(a => a.OwnerId == ownerId)
            .OrderByDescending(a => a.CreatedAt);
        var totalCount = await query.CountAsync();
        var data = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
        return new PagedResult<Appointment>(data, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public void Update(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
    }

    public void Delete(Appointment appointment)
    {
        _context.Appointments.Remove(appointment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}