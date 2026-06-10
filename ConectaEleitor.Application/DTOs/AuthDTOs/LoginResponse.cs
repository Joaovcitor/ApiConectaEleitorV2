namespace ConectaEleitor.Application.DTOs.AuthDTOs;

public class LoginResponse
{
    public string Message { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string CompleteName { get; set; } = string.Empty;
    public Guid? OwnerId { get; set; }
}