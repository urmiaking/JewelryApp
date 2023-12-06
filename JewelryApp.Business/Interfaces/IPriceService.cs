using JewelryApp.Models.Dtos.PriceDtos;

namespace JewelryApp.Business.Interfaces;

public interface IPriceService
{
    Task<PriceDto> GetPriceAsync(CancellationToken token = default);
}