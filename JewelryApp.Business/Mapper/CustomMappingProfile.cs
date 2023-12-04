using AutoMapper;
using JewelryApp.Common.Interfaces;

namespace JewelryApp.Business.Mapper;

public class CustomMappingProfile : Profile
{
    public CustomMappingProfile(IEnumerable<IHaveCustomMapping> customMappings)
    {
        foreach (var customMapping in customMappings)
            customMapping.CreateMappings(this);
    }
}
