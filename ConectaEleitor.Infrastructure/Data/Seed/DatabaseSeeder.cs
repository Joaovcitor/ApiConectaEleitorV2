using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Domain.Enums;
using ConectaEleitor.Infrastructure.Data.Context;
using ConectaEleitor.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        var dbContext =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await SeedRoles(roleManager);

        await SeedAdmin(userManager, configuration);

        await SeedPlans(dbContext);
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

    private static async Task SeedPlans(AppDbContext dbContext)
    {
        if (await dbContext.Plans.AnyAsync())
            return;

        var starter = CreatePlan(
            "Starter",
            "starter",
            "Plano inicial para operações menores.",
            99,
            990,
            1,
            [
                ("Relatórios básicos", "basic_reports", "Acesso a relatórios básicos")
            ],
            [
                (PlanLimitType.Users, 3, false),
                (PlanLimitType.Voters, 5000, false),
                (PlanLimitType.Leaders, 50, false),
                (PlanLimitType.Demands, 1000, false),
                (PlanLimitType.Appointments, 200, false)
            ]);

        var professional = CreatePlan(
            "Professional",
            "professional",
            "Plano profissional para equipes em crescimento.",
            299,
            2990,
            2,
            [
                ("Relatórios avançados", "advanced_reports", "Acesso a relatórios avançados"),
                ("Exportações", "exports", "Exportação de dados")
            ],
            [
                (PlanLimitType.Users, 10, false),
                (PlanLimitType.Voters, 30000, false),
                (PlanLimitType.Leaders, 300, false),
                (PlanLimitType.Demands, null, true),
                (PlanLimitType.Appointments, null, true)
            ]);

        var enterprise = CreatePlan(
            "Enterprise",
            "enterprise",
            "Plano corporativo com limites ampliados.",
            0,
            null,
            3,
            [
                ("Suporte prioritário", "priority_support", "Atendimento prioritário")
            ],
            [
                (PlanLimitType.Users, null, true),
                (PlanLimitType.Voters, null, true),
                (PlanLimitType.Leaders, null, true),
                (PlanLimitType.Demands, null, true),
                (PlanLimitType.Appointments, null, true)
            ]);

        await dbContext.Plans.AddRangeAsync(starter, professional, enterprise);
        await dbContext.SaveChangesAsync();
    }

    private static Plan CreatePlan(
        string name,
        string slug,
        string description,
        decimal monthlyPrice,
        decimal? yearlyPrice,
        int displayOrder,
        IEnumerable<(string Name, string Key, string Description)> features,
        IEnumerable<(PlanLimitType Type, int? Value, bool IsUnlimited)> limits)
    {
        var planId = Guid.NewGuid();

        return new Plan
        {
            PlanId = planId,
            Name = name,
            Slug = slug,
            Description = description,
            MonthlyPrice = monthlyPrice,
            YearlyPrice = yearlyPrice,
            DisplayOrder = displayOrder,
            CreatedAt = DateTime.UtcNow,
            Features = features.Select(feature => new PlanFeature
            {
                PlanFeatureId = Guid.NewGuid(),
                PlanId = planId,
                Name = feature.Name,
                Key = feature.Key,
                Description = feature.Description,
                IsEnabled = true
            }).ToList(),
            Limits = limits.Select(limit => new PlanLimit
            {
                PlanLimitId = Guid.NewGuid(),
                PlanId = planId,
                Type = limit.Type,
                Value = limit.Value,
                IsUnlimited = limit.IsUnlimited
            }).ToList()
        };
    }
}
