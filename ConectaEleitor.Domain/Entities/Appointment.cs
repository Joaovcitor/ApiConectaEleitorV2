using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Domain.Entities;

public class Appointment
{
    public Guid AppointmentId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }

    public string? Location { get; set; }

    public AppointmentStatus Status { get; set; }

    public Guid? CitizenId { get; set; }
    public Citizen? Citizen { get; set; }
    
    public Guid AgendaId { get; set; }
    public Agenda Agenda { get; set; } = null!;
    
    public Guid OwnerId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}