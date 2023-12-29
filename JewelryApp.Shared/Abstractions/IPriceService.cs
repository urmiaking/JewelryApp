using ErrorOr;
using JewelryApp.Shared.Responses.Prices;

namespace JewelryApp.Shared.Abstractions;

public interface IPriceService
{
    Task<ErrorOr<PriceResponse?>> GetPriceAsync(CancellationToken token = default);
}