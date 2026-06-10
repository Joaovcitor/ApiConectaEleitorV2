namespace ConectaEleitor.Application.Interfaces;

public interface IUserContext
{
    Guid UserId { get; }
    Guid OwnerId { get; }
    string? Email { get; }
    string? Name { get; }
    IReadOnlyList<string> Roles { get; }

    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
    bool IsLeader { get; }
}