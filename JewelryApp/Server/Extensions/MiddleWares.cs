using JewelryApp.Business.Repositories.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace JewelryApp.Api.Extensions;

public static class MiddleWares
{
    public static void UseInitializer(this WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();

        dbInitializer!.Initialize();
    }
}