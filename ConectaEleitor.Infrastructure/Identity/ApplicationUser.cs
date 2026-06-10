using Microsoft.AspNetCore.Identity;

namespace ConectaEleitor.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string CompleteName { get; set; } = string.Empty;
    public Guid? OwnerId { get; set; }
}