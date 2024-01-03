using JewelryApp.Shared.Responses.Prices;

namespace JewelryApp.Shared.Abstractions;

public interface IPriceService
{
    Task<PriceResponse?> GetPriceAsync(CancellationToken token = default);
}