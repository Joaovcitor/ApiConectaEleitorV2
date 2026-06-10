namespace ConectaEleitor.Domain.Entities;

public class Agenda
{
    public Guid AgendaId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = [];

    public Guid OwnerId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}