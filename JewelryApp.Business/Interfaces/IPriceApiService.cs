using JewelryApp.Models.Dtos.PriceDtos.Signal;

namespace JewelryApp.Business.Interfaces;

public interface IPriceApiService
{
    Task<PriceApiResult?> GetPriceAsync(CancellationToken token = default);
}