namespace ConectaEleitor.Application.DTOs.PlanUsages;

public class PlanUsageUpdateDTO
{
    public int UsersCount { get; set; }
    public int VotersCount { get; set; }
    public int LeadersCount { get; set; }
    public int DemandsCount { get; set; }
    public int AppointmentsCount { get; set; }
    public int ReportsGeneratedCount { get; set; }
    public int ExportsGeneratedCount { get; set; }
    public long StorageUsedBytes { get; set; }
}
