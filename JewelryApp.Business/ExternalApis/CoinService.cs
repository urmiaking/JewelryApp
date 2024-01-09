using JewelryApp.Application.ExternalApis.Abstraction;
using JewelryApp.Application.ExternalModels.Signal;
using JewelryApp.Core.Constants;
using JewelryApp.Core.Utilities;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace JewelryApp.Application.ExternalApis;

public class CoinService : ICoinService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoinService> _logger;

    public CoinService(HttpClient httpClient, ILogger<CoinService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PriceApiResult> GetCoinPriceAsync(CancellationToken token = default)
    {
        try
        {
            var payload = new SignalApiBody
            {
                Property = new[] { "id", "name", "close" },
                SortBy = "index",
                Desc = false,
                Market = "coin"
            };

            var response = await _httpClient.PostAsJsonAsync(AppConstants.SignalApiUrl, payload, cancellationToken: token);

            if (!response.IsSuccessStatusCode)
                return new PriceApiResult();

            var apiResult = await response.Content.ReadFromJsonAsync<SignalApiResult>(cancellationToken: token);

            var dataList = apiResult?.Data.InnerData?.Where(x => x.Id is >= 100000 and <= 100027).ToList();

            if (dataList is null || dataList.Count == 0)
                return new PriceApiResult();

            var priceApiResult = new PriceApiResult
            {
                DateTime = apiResult?.Meta.ShamsiDate.FormatShamsiDateTime()
            };

            foreach (var coinData in dataList)
                switch (coinData.Id)
                {
                    case 100001:
                        priceApiResult.CoinImam = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100002:
                        priceApiResult.CoinNim = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100003:
                        priceApiResult.CoinRob = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100000:
                        priceApiResult.CoinBahar = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100004:
                        priceApiResult.CoinGrami = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100024:
                        priceApiResult.CoinParsian500Sowt = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100023:
                        priceApiResult.CoinParsian400Sowt = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100022:
                        priceApiResult.CoinParsian300Sowt = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100021:
                        priceApiResult.CoinParsian250Sowt = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100020:
                        priceApiResult.CoinParsian200Sowt = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100019:
                        priceApiResult.CoinParsian150Sowt = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100018:
                        priceApiResult.CoinParsian100Sowt = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100017:
                        priceApiResult.CoinParsian50Sowt = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100025:
                        priceApiResult.CoinParsian1Gram = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100027:
                        priceApiResult.CoinParsian15Gram = coinData.Close.RemoveLeadingZero();
                        break;
                    case 100026:
                        priceApiResult.CoinParsian2Gram = coinData.Close.RemoveLeadingZero();
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