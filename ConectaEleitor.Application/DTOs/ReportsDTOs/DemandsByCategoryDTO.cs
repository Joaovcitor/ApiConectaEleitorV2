namespace ConectaEleitor.Application.DTOs.ReportsDTOs;

public class DemandsByCategoryDTO
{
    public Guid? CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int Total { get; set; }
}