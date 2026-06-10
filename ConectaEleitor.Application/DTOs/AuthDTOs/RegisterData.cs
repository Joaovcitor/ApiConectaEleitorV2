namespace ConectaEleitor.Application.DTOs.AuthDTOs;

public class RegisterData
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string CompleteName  { get; set; } = string.Empty;
}