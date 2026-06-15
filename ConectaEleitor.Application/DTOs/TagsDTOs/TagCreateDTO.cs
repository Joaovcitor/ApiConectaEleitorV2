namespace ConectaEleitor.Application.DTOs.TagsDTOs;

public class TagCreateDTO
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Color { get; set; }
}