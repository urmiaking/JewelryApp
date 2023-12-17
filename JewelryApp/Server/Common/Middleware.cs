using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Utilities;
using JewelryApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Api.Common;

public static class Middleware
{
    public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
    {
        Assert.NotNull(app, nameof(app));

        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var appDbContext = scope.ServiceProvider.GetService<AppDbContext>();

        appDbContext?.Database.Migrate();

        var initializerList = scope.ServiceProvider.GetServices<IDbInitializer>();

        foreach (var dataInitializer in initializerList)
        {
            dataInitializer.Initialize();
        }

        return app;
    }
}

