using ErrorOr;
using JewelryApp.Shared.Responses.Prices;

namespace JewelryApp.Business.Interfaces;

public interface IPriceService
{
    Task<ErrorOr<PriceResponse?>> GetPriceAsync(CancellationToken token = default);
}