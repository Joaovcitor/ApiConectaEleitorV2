using ConectaEleitor.API.Middlewares;
using ConectaEleitor.Application.Interfaces;
using ConectaEleitor.Application.Mappings;
using ConectaEleitor.Application.Services;
using ConectaEleitor.Infrastructure.Data.Context;
using ConectaEleitor.Infrastructure.Identity;
using ConectaEleitor.Infrastructure.Repositories;
using ConectaEleitor.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ConectaEleitor.Infrastructure.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DemandMappingProfile>();
    cfg.AddProfile<AppointmentMappingProfile>();
    cfg.AddProfile<CitizenMappingProfile>();
    cfg.AddProfile<AgendaMappingProfile>();
});

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "conecta_eleitor_auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.LoginPath = "/api/auth/login";
    options.LogoutPath = "/api/auth/logout";
    options.AccessDeniedPath = "/api/auth/access-denied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AssemblymanFunctions", policy =>
        policy.RequireRole("Assemblyman"));
    options.AddPolicy("OnlyAdmin", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserContext, UserContext>();

// Services application
builder.Services.AddScoped<ICitizenService, CitizenService>();
builder.Services.AddScoped<IAgendaService, AgendaService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDemandService, DemandService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReportService, ReportService>();
// Repositories
builder.Services.AddScoped<ICitizenRepository, CitizenRepository>();
builder.Services.AddScoped<IAgendaRepository, AgendaRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IDemandRepository, DemandRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
var app = builder.Build();
await DatabaseSeeder.SeedAsync(
    app.Services,
    builder.Configuration);
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("Default");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();