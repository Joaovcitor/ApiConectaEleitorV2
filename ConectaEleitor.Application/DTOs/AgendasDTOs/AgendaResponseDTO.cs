namespace ConectaEleitor.Application.DTOs.AgendasDTOs;

public class AgendaResponseDTO
{
    public Guid AgendaId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int TotalAppointments { get; set; }

    public DateTime CreatedAt { get; set; }
}