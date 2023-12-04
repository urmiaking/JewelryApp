using AutoMapper;

namespace JewelryApp.Common.Interfaces;

public interface IHaveCustomMapping
{
    void CreateMappings(Profile profile);
}