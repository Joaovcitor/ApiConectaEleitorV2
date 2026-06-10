namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class LeaderRankingDTO
{
    public Guid LeaderId { get; set; }
    public string LeaderName { get; set; } = string.Empty;
    public int TotalLedCitizens { get; set; }
}