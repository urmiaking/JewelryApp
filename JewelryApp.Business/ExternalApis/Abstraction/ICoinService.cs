using JewelryApp.Application.ExternalModels.Signal;

namespace JewelryApp.Application.ExternalApis.Abstraction;

public interface ICoinService
{
    Task<PriceApiResult> GetCoinPriceAsync(CancellationToken token = default);
}