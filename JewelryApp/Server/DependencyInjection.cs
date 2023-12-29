using JewelryApp.Core.Settings;
using System.Reflection;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Extensions;

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
        services.AddHttpContextAccessor();

        services.DiscoverServices();

        return services;
    }

    private static void DiscoverServices(this IServiceCollection services)
    {
        var assembliesToScan = new[]
        {
            Assembly.GetExecutingAssembly(),
            Assembly.Load("JewelryApp.Infrastructure"),
            Assembly.Load("JewelryApp.Application")
        };

        services.DiscoverSingletonServices(assembliesToScan);
        services.DiscoverScopedServices(assembliesToScan);
    }
}