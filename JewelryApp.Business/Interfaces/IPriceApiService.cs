using JewelryApp.Business.ExternalModels.Signal;

namespace JewelryApp.Business.Interfaces;

public interface IPriceApiService
{
    Task<PriceApiResult?> GetPriceAsync(CancellationToken token = default);
}