using ErrorOr;
using JewelryApp.Shared.Responses.Prices;

namespace JewelryApp.Application.Interfaces;

public interface IPriceService
{
    Task<ErrorOr<PriceResponse?>> GetPriceAsync(CancellationToken token = default);
}