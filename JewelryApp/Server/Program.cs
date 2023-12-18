using JewelryApp.Api;
using JewelryApp.Api.Common;
using JewelryApp.Application;
using JewelryApp.Application.Hubs;
using JewelryApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApi(builder.Configuration)
        .AddApplication(builder.Configuration)
        .AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.InitializeDatabase();
#if DEBUG
    app.UseWebAssemblyDebugging();
#endif
    app.UseHttpsRedirection();
    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapRazorPages();
    app.MapControllers().RequireAuthorization();

    app.MapHub<PriceHub>("/signalr-hub");

    app.MapFallbackToFile("index.html");

    app.Run();
}