namespace ConectaEleitor.Application.DTOs.TagCitizensDTOs;

public class TagCitizenCreateDTO
{
    public Guid TagId { get; set; }
    public Guid CitizenId { get; set; }
    public string? Reason { get; set; }
}