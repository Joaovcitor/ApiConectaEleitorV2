using ConectaEleitor.Application.DTOs.AuthDTOs;

namespace ConectaEleitor.Application.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAssessor(RegisterData registerData);
    Task<string> RegisterAssemblyman(RegisterData registerData);
    Task<LoginResponse> Login(LoginData loginData);
    Task Logout();
    Task<MeResponse> GetMe();
}