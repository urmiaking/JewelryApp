using JewelryApp.Application.ExternalApis.Abstraction;
using JewelryApp.Application.ExternalModels.Signal;
using JewelryApp.Core.Constants;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using JewelryApp.Core.Utilities;

namespace JewelryApp.Application.ExternalApis;

public class GoldService : IGoldService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoinService> _logger;

    public GoldService(HttpClient httpClient, ILogger<CoinService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PriceApiResult> GetGoldPriceAsync(CancellationToken token = default)
    {
        try
        {
            var payload = new SignalApiBody
            {
                Property = new[] { "id", "name", "close" },
                SortBy = "index",
                Desc = false,
                Market = "gold"
            };

            var response = await _httpClient.PostAsJsonAsync(AppConstants.SignalApiUrl, payload, cancellationToken: token);

            if (!response.IsSuccessStatusCode)
                return new PriceApiResult();

            var apiResult = await response.Content.ReadFromJsonAsync<SignalApiResult>(cancellationToken: token);

            var dataList = apiResult?.Data.InnerData?.Where(x => x.Id is >= 100010 and <= 100014).ToList();

            if (dataList is null || dataList.Count == 0)
                return new PriceApiResult();

            var priceApiResult = new PriceApiResult
            {
                DateTime = apiResult?.Meta.ShamsiDate.FormatShamsiDateTime()
            };

            foreach (var goldData in dataList)
                switch (goldData.Id)
                {
                    case 100010:
                        priceApiResult.Gram17 = goldData.Close.RemoveLeadingZero();
                        break;
                    case 100011:
                        priceApiResult.Gram18 = goldData.Close.RemoveLeadingZero();
                        break;
                    case 100012:
                        priceApiResult.Gram24 = goldData.Close.RemoveLeadingZero();
                        break;
                    case 100013:
                        priceApiResult.Mazanneh = goldData.Close.RemoveLeadingZero();
                        break;
                    case 100014:
                        priceApiResult.Mesghal = goldData.Close.RemoveLeadingZero();
                        break;
                }

            return priceApiResult;
        }
        catch (Exception e)
        {
            _logger.LogWarning(e.Message);
            return new PriceApiResult();
        }
    }
}