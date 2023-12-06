﻿using JewelryApp.Common.Constants;
using JewelryApp.Common.Utilities;
using JewelryApp.Data.Interfaces;
using JewelryApp.Data;
using Microsoft.EntityFrameworkCore;
using JewelryApp.Models.AppModels;
using JewelryApp.Common.Settings;

namespace JewelryApp.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddRazorPages();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddSignalR();
        services.AddHttpContextAccessor();

        return services;
    }

    public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
    {
        Assert.NotNull(app, nameof(app));

        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var appDbContext = scope.ServiceProvider.GetService<AppDbContext>();

        var retryCount = 0;

        try
        {
            //Applies any pending migrations for the context to the database like (Update-Database)
            appDbContext?.Database.Migrate();

            var dataInitializers = scope.ServiceProvider.GetServices<IDbInitializer>();

            foreach (var dataInitializer in dataInitializers)
            {
                dataInitializer.Initialize();
            }
        }
        catch (Exception e)
        {
            if (retryCount < AppConstants.RetryCount)
            {
                retryCount++;
                var loggerFactory = scope.ServiceProvider.GetService<ILoggerFactory>();
                var logger = loggerFactory?.CreateLogger<Program>();
                logger?.LogError(e.Message);
                InitializeDatabase(app);
            }
            throw;
        }

        return app;
    }
}
