using System.Reflection;
using JewelryApp.Business.AppServices;
using JewelryApp.Business.Repositories.Implementations;
using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Data;
using JewelryApp.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using JewelryApp.Business.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NCrontab;

namespace JewelryApp.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCustomIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 5;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        });
    }

    public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<TokenValidationParameters>(_ => GetTokenValidationParameters(configuration));

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = GetTokenValidationParameters(configuration);
        });
        services.AddAuthorization();
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IDbInitializer, DbInitializer>();
        services.AddScoped<IBarcodeRepository, BarcodeRepository>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddHttpClient<IPriceRepository, PriceRepository>();
        services.AddScoped<IRepository<RefreshToken>, RefreshTokenRepository>();
        services.AddScoped<IRepository<Invoice>, InvoiceRepository>();
        services.AddScoped<IRepository<InvoiceProduct>, InvoiceProductRepository>();
        services.AddScoped<IRepository<Product>, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
    }

    public static void RegisterAutoMapper(this IServiceCollection services)
    {
        var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies()
            .Select(Assembly.Load)
            .SelectMany(a => a.DefinedTypes)
            .Where(t => typeof(Profile).IsAssignableFrom(t.AsType()))
            .Select(t => t.AsType()).ToArray();

        services.AddAutoMapper(assemblies);
    }

    public static void AddPriceUpdateJob(this IServiceCollection services, IConfiguration configuration)
    {
        var cronExpression = configuration.GetSection("CronExpression").Value 
                             ?? throw new ArgumentException("There is no cron expression");

        services.AddCronJob<UpdatePriceJob>(cronExpression);
    }

    public static IServiceCollection AddCronJob<T>(this IServiceCollection services, string cronExpression)
        where T : class, ICronJob
    {
        var cron = CrontabSchedule.TryParse(cronExpression)
                   ?? throw new ArgumentException("Invalid cron expression", nameof(cronExpression));

        var entry = new CronRegistryEntry(typeof(T), cron);

        services.AddHostedService<CronScheduler>();
        services.TryAddSingleton<T>();
        services.AddSingleton(entry);

        return services;
    }

    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default") 
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AppDbContext>(options => options
            .UseSqlServer(connectionString));
    }

    private static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
    {
        return new TokenValidationParameters()
        {
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidAudience = configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,

        };
    }

}