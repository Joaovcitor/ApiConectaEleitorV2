using ConectaEleitor.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConectaEleitor.Infrastructure.Data.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager =
            scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        var userManager =
            scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await SeedRoles(roleManager);

        await SeedAdmin(userManager, configuration);
    }

    private static async Task SeedRoles(
        RoleManager<ApplicationRole> roleManager)
    {
        string[] roles =
        [
            "Admin",
            "Assemblyman",
            "Assessor",
            "Leader"
        ];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role
                });
            }
        }
    }

    private static async Task SeedAdmin(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        var section = configuration.GetSection("SeedAdmin");

        var email = section["Email"];
        var password = section["Password"];
        var completeName = section["CompleteName"];

        if (string.IsNullOrWhiteSpace(email))
            return;

        var admin = await userManager.FindByEmailAsync(email);

        if (admin is not null)
            return;

        admin = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            CompleteName = completeName ?? "Administrador",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, password!);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ",
                result.Errors.Select(x => x.Description));

            throw new Exception($"Erro ao criar usuário administrador: {errors}");
        }
        
        await userManager.UpdateAsync(admin);

        await userManager.AddToRoleAsync(admin, "Admin");
    }
}