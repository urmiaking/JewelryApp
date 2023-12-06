﻿using JewelryApp.Business.Repositories.Implementations;
using JewelryApp.Common.Constants;
using JewelryApp.Data.Implementations.Repositories;
using JewelryApp.Data.Interfaces.Repositories.Base;
using JewelryApp.Data.Interfaces.Repositories;
using JewelryApp.Data.Interfaces;
using JewelryApp.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using JewelryApp.Data.Implementations.Repositories.Base;

namespace JewelryApp.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<AppDbContext>(configuration.GetConnectionString(AppConstants.ConnectionStringName));
        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IPriceRepository, PriceRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IDbInitializer, DbInitializer>();
        services.AddAppIdentity();
        services.AddCustomAuthentication(configuration);

        return services;
    }

    private static void AddAppIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>()
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
        services.AddTransient(_ => GetTokenValidationParameters(configuration));

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
            ClockSkew = TimeSpan.Zero
        };
    }
}