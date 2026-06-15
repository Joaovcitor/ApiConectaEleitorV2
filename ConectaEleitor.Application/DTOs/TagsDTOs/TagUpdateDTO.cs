namespace ConectaEleitor.Application.DTOs.TagsDTOs;

public class TagUpdateDTO
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Color { get; set; }
}