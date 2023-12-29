using JewelryApp.Shared.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JewelryApp.Shared.Extensions;

public static class DependencyInjection
{
    public static void DiscoverSingletonServices(this IServiceCollection services, IEnumerable<Assembly> assemblies)
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

    public static void DiscoverScopedServices(this IServiceCollection services, IEnumerable<Assembly> assemblies)
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