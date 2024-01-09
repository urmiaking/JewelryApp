using JewelryApp.Application.ExternalApis.Abstraction;
using JewelryApp.Application.ExternalModels.Signal;
using JewelryApp.Core.Constants;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using JewelryApp.Core.Utilities;

namespace JewelryApp.Application.ExternalApis;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoinService> _logger;

    public CurrencyService(HttpClient httpClient, ILogger<CoinService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PriceApiResult> GetCurrencyAsync(CancellationToken token = default)
    {
        try
        {
            var payload = new SignalApiBody
            {
                Property = new[] { "id", "name", "close" },
                SortBy = "index",
                Desc = false,
                Market = "currency"
            };

            var response = await _httpClient.PostAsJsonAsync(AppConstants.SignalApiUrl, payload, cancellationToken: token);

            if (!response.IsSuccessStatusCode)
                return new PriceApiResult();

            var apiResult = await response.Content.ReadFromJsonAsync<SignalApiResult>(cancellationToken: token);

            var dataList = apiResult?.Data.InnerData?.Where(x => x.Id is >= 200000 and <= 200001).ToList();

            if (dataList is null || dataList.Count == 0)
                return new PriceApiResult();

            var priceApiResult = new PriceApiResult
            {
                UsDollar = dataList.FirstOrDefault(x => x.Id == 200000)!.Close.RemoveLeadingZero(),
                UsEuro = dataList.FirstOrDefault(x => x.Id == 200001)!.Close.RemoveLeadingZero(),
                DateTime = apiResult?.Meta.ShamsiDate.FormatShamsiDateTime()
        };

            return priceApiResult;
        }
        catch (Exception e)
        {
            _logger.LogWarning(e.Message);
            return new PriceApiResult();
        }
    }
}