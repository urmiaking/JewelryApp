using AutoMapper;
using JewelryApp.Common.Interfaces;

namespace JewelryApp.Models.Dtos.Price.Signal;

public class PriceApiResult : IHaveCustomMapping
{
    public double Gram17 { get; set; }
    public double Gram18 { get; set; }
    public double Gram24 { get; set; }
    public double Mazanneh { get; set; }
    public double Mesghal { get; set; }
    public double UsDollar { get; set; }

    public void CreateMappings(Profile profile)
    {
        profile.CreateMap<PriceApiResult, Price>().ReverseMap();
    }
}