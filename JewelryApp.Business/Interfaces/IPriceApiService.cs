using JewelryApp.Application.ExternalModels.Signal;

namespace JewelryApp.Application.Interfaces;

public interface IPriceApiService
{
    Task<PriceApiResult?> GetPriceAsync(CancellationToken token = default);
}