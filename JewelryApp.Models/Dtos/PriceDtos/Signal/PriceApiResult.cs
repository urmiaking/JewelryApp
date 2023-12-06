using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.CommonDtos;

namespace JewelryApp.Models.Dtos.PriceDtos.Signal;

public class PriceApiResult : BaseDto<PriceApiResult, Price>
{
    public double Gram17 { get; set; }
    public double Gram18 { get; set; }
    public double Gram24 { get; set; }
    public double Mazanneh { get; set; }
    public double Mesghal { get; set; }
    public double UsDollar { get; set; }
}