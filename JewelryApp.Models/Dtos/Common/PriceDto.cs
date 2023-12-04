namespace JewelryApp.Models.Dtos.Common;

public class PriceDto : BaseDto<PriceDto, Data.Models.Price>
{
    public double Gram17 { get; set; }
    public double Gram18 { get; set; }
    public double Gram24 { get; set; }
    public double Mazanneh { get; set; }
    public double Mesghal { get; set; }
    public double UsDollar { get; set; }
}