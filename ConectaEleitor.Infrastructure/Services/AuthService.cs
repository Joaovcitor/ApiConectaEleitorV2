using System.Security.Claims;
using ConectaEleitor.Application.DTOs.AuthDTOs;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Domain.Exceptions;
using ConectaEleitor.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ConectaEleitor.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserContext _userContext;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
        IUserContext userContext, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userContext = userContext;
        _roleManager = roleManager;
    }

    public async Task<string> RegisterAssessor(RegisterData registerData)
    {
        if (string.IsNullOrWhiteSpace(registerData.Email))
        {
            throw new BadRequestException("Email é obrigatório!");
        }

        if (string.IsNullOrWhiteSpace(registerData.Password))
        {
            throw new BadRequestException("Senha não pode ficar em branco");
        }

        if (registerData.Password != registerData.ConfirmPassword)
        {
            throw new BadRequestException("Senhas não coincidem");
        }
        var ownerId = _userContext.UserId;
        var userExist = await _userManager.FindByEmailAsync(registerData.Email);
        if (userExist != null)
        {
            throw new ConflictException("Usuário já existe no sistema!");
        }

        var user = new ApplicationUser
        {
            CompleteName = registerData.CompleteName,
            Email = registerData.Email,
            UserName = registerData.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            OwnerId = ownerId,
            EmailConfirmed = true
        };
        var result = await _userManager.CreateAsync(user, registerData.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(" | ", result.Errors.Select(error => error.Description));
            throw new BadRequestException(errors);
        }
        await _userManager.AddToRoleAsync(user, "Assessor");
        return "Assessor criado com sucesso!";
    }

    public async Task<string> RegisterAssemblyman(RegisterData registerData)
    {
        if (string.IsNullOrWhiteSpace(registerData.Email))
        {
            throw new BadRequestException("Email é obrigatório!");
        }

        if (string.IsNullOrWhiteSpace(registerData.Password))
        {
            throw new BadRequestException("Senha não pode ficar em branco");
        }

        if (registerData.Password != registerData.ConfirmPassword)
        {
            throw new BadRequestException("Senhas não coincidem");
        }
        var userExist = await _userManager.FindByEmailAsync(registerData.Email);
        if (userExist != null)
        {
            throw new ConflictException("Usuário já existe no sistema!");
        }

        var user = new ApplicationUser
        {
            CompleteName = registerData.CompleteName,
            Email = registerData.Email,
            UserName = registerData.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true
        };
        var result = await _userManager.CreateAsync(user, registerData.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(" | ", result.Errors.Select(error => error.Description));
            throw new BadRequestException(errors);
        }
        await _userManager.AddToRoleAsync(user, "Assemblyman");
        return "Vereador criado com sucesso!";
    }

    public async Task<LoginResponse> Login(LoginData loginData)
    {
        var user = await _userManager.FindByEmailAsync(loginData.Email);
        if (user == null)
        {
            throw new BadRequestException("E-mail ou senha incorretos!");
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginData.Password, true);
        if (result.IsLockedOut)
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            var minutes = lockoutEnd.HasValue
                ? Math.Ceiling((lockoutEnd.Value - DateTimeOffset.UtcNow).TotalMinutes)
                : 0;
            throw new UnauthorizedException($"Sua conta foi bloqueada temporariamente, aguarde {minutes} minutos");
        }
        if(!result.Succeeded)
            throw new BadRequestException("E-mail ou senha incorretos!");
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.CompleteName),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new("ownerId", user.OwnerId.ToString())
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        await _signInManager.SignInWithClaimsAsync(
            user,
            isPersistent: true,
            claims);
        return new LoginResponse
        {
            Message = "Login efetuado com sucesso!",
            CompleteName = user.CompleteName,
            Email = user.Email!,
            UserId = user.Id,
            OwnerId = user.OwnerId
        };
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<MeResponse> GetMe()
    {
        var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString());

        if (user == null)
            throw new UnauthorizedException();

        var roles = await _userManager.GetRolesAsync(user);

        return new MeResponse()
        {
           UserId =  user.Id,
           CompleteName = user.CompleteName,
           Email = user.Email,
           Roles = roles.ToList(),
        };
    }
}