using JewelryApp.Application.ExternalModels.Signal;

namespace JewelryApp.Application.ExternalApis.Abstraction;

public interface ICurrencyService
{
    Task<PriceApiResult> GetCurrencyAsync(CancellationToken token = default);
}