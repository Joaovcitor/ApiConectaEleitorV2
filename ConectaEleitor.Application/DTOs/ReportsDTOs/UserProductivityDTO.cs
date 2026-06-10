namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class UserProductivityDTO
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;

    public int CitizensCreated { get; set; }
    public int DemandsCreated { get; set; }
    public int DemandsCompleted { get; set; }
}