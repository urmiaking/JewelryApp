using ErrorOr;
using JewelryApp.Client.Services.Extensions;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Responses.Prices;

namespace JewelryApp.Client.Services.HttpServices;

[ScopedService<IPriceService>]
public class PriceService : IPriceService
{
    private readonly HttpClient _authorizedClient;

    public PriceService(IHttpClientFactory httpClientFactory)
    {
        _authorizedClient = httpClientFactory.CreateClient("AuthorizedClient");
    }

    public async Task<PriceResponse?> GetPriceAsync(CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync(Urls.Price, token);

            return await response.GenerateResponseAsync<PriceResponse?>(token);
        }
        catch
        {
            return null;
        }
    }
}