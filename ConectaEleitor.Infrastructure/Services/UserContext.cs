using System.Security.Claims;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ConectaEleitor.Infrastructure.Services;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User =>
        _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            var value = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(value, out var userId)
                ? userId
                : Guid.Empty;
        }
    }

    public Guid OwnerId
    {
        get
        {
            var value = User?.FindFirstValue("ownerId");

            return Guid.TryParse(value, out var ownerId)
                ? ownerId
                : UserId;
        }
    }

    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email);

    public string? Name =>
        User?.FindFirstValue(ClaimTypes.Name);

    public IReadOnlyList<string> Roles =>
        User?.FindAll(ClaimTypes.Role)
            .Select(x => x.Value)
            .ToList() ?? [];

    public bool IsAdmin =>
        Roles.Contains("Admin");

    public bool IsLeader =>
        Roles.Contains("Leader");
}