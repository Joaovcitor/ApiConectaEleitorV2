using ConectaEleitor.Domain.Enums;

namespace ConectaEleitor.Application.DTOs.AppointmentsDTOs;

public class AppointmentResponseDTO
{
    public Guid AppointmentId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }

    public string? Location { get; set; }

    public AppointmentStatus Status { get; set; }

    public Guid AgendaId { get; set; }
    public string AgendaName { get; set; } = string.Empty;

    public Guid? CitizenId { get; set; }
    public string? CitizenName { get; set; }

    public DateTime CreatedAt { get; set; }
}