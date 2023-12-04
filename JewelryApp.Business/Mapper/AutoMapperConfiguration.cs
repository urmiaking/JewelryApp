using System.Reflection;
using AutoMapper;
using JewelryApp.Common.Interfaces;

namespace JewelryApp.Business.Mapper;

public static class AutoMapperConfiguration
{
    public static void AddCustomMappingProfile(this IMapperConfigurationExpression config)
    {
        config.AddCustomMappingProfile(AppDomain.CurrentDomain.GetAssemblies());
    }

    public static void AddCustomMappingProfile(this IMapperConfigurationExpression config, params Assembly[] assemblies)
    {
        var allTypes = assemblies.SelectMany(a => a.ExportedTypes);

        var list = allTypes.Where(type => type is { IsClass: true, IsAbstract: false } &&
                                          type.GetInterfaces().Contains(typeof(IHaveCustomMapping)))
            .Select(type => Activator.CreateInstance(type) as IHaveCustomMapping);

        var profile = new CustomMappingProfile(list);

        config.AddProfile(profile);
    }
}
