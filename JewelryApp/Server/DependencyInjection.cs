using JewelryApp.Core.Settings;
using JewelryApp.Core.Attributes;
using System.Reflection;

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

        services.DiscoverServices(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    private static void DiscoverServices(this IServiceCollection services, Assembly[] assembly)
    {
        var assembliesToScan = new[]
        {
            Assembly.GetExecutingAssembly(), // The current assembly (e.g. API)
            Assembly.Load("JewelryApp.Infrastructure"), // Your Infrastructure assembly
            Assembly.Load("JewelryApp.Application") // Your Application assembly
        };

        services.DiscoverSingletonServices(assembliesToScan);
        services.DiscoverScopedServices(assembliesToScan);
    }

    private static void DiscoverSingletonServices(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var typesList = assemblies.Select(assembly => assembly.GetTypes()).ToList();

        foreach (var types in typesList)
        {
            foreach (var type in types)
            {
                var serviceAttrs = type.GetCustomAttributes()
                    .Where(x => x.GetType().IsGenericType && x.GetType().GetGenericTypeDefinition() == typeof(SingletonServiceAttribute<>));

                foreach (var serviceAttr in serviceAttrs)
                {
                    var implementationType = type;
                    var serviceType = serviceAttr.GetType().GenericTypeArguments[0];

                    services.AddSingleton(serviceType, implementationType);
                }
            }
        }
    }

    private static void DiscoverScopedServices(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var typesList = assemblies.Select(assembly => assembly.GetTypes()).ToList();

        foreach (var types in typesList)
        {
            foreach (var type in types)
            {
                var serviceAttrs = type.GetCustomAttributes()
                    .Where(x => x.GetType().IsGenericType && x.GetType().GetGenericTypeDefinition() == typeof(ScopedServiceAttribute<>));

                foreach (var serviceAttr in serviceAttrs)
                {
                    var implementationType = type;
                    var serviceType = serviceAttr.GetType().GenericTypeArguments[0];

                    services.AddScoped(serviceType, implementationType);
                }
            }
        }
    }
}