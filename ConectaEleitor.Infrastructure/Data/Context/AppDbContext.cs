using ConectaEleitor.Domain.Entities;
using ConectaEleitor.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConectaEleitor.Infrastructure.Data.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Citizen> Citizens { get; set; }
    public DbSet<Agenda> Agendas { get; set; }
    public DbSet<Demand>  Demands { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<PlanFeature> PlanFeatures { get; set; }
    public DbSet<PlanLimit> PlanLimits { get; set; }
    public DbSet<OwnerSubscription> OwnerSubscriptions { get; set; }
    public DbSet<SubscriptionHistory> SubscriptionHistory { get; set; }
    public DbSet<PlanUsage> PlanUsages { get; set; }
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    public DbSet<AssemblymanPost> AssemblymanPosts { get; set; }
    public DbSet<AssemblymanPostComment> AssemblymanPostComments { get; set; }
    public DbSet<AssemblymanPostLike> AssemblymanPostLikes { get; set; }
    public DbSet<AssemblymanPostCategory> AssemblymanPostCategories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TagCitizen> TagCitizens { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}