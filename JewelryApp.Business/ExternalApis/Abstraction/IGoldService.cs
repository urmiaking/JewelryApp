using JewelryApp.Application.ExternalModels.Signal;

namespace JewelryApp.Application.ExternalApis.Abstraction;

public interface IGoldService
{
    Task<PriceApiResult> GetGoldPriceAsync(CancellationToken token = default);

}