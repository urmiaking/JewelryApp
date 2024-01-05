using System.Net.Http.Json;
using JewelryApp.Application.ExternalModels.Signal;
using JewelryApp.Application.Interfaces;
using JewelryApp.Core.Constants;
using JewelryApp.Core.Utilities;

namespace JewelryApp.Application.AppServices;

public class PriceApiService : IPriceApiService
{
    private readonly HttpClient _httpClient;

    public PriceApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PriceApiResult?> GetPriceAsync(CancellationToken token)
    {
        var payload = new SignalApiBody
        {
            Property = new[] { "id", "name", "close" },
            SortBy = "index",
            Desc = false,
            Market = "currency"
        };

        var signalCurrencyResponse = await _httpClient.PostAsJsonAsync(AppConstants.SignalApiUrl, payload, cancellationToken: token);

        payload.Market = "gold";
        var signalGoldResponse = await _httpClient.PostAsJsonAsync(AppConstants.SignalApiUrl, payload, cancellationToken: token);

        payload.Market = "coin";
        var signalCoinResponse = await _httpClient.PostAsJsonAsync(AppConstants.SignalApiUrl, payload, cancellationToken: token);

        if (!signalCurrencyResponse.IsSuccessStatusCode || !signalGoldResponse.IsSuccessStatusCode || !signalCoinResponse.IsSuccessStatusCode)
            return null;

        var signalCurrencyApiResult = await signalCurrencyResponse.Content.ReadFromJsonAsync<SignalApiResult>(cancellationToken: token);

        var signalGoldApiResult = await signalGoldResponse.Content.ReadFromJsonAsync<SignalApiResult>(cancellationToken: token);

        var signalCoinApiResult = await signalCoinResponse.Content.ReadFromJsonAsync<SignalApiResult>(cancellationToken: token);

        var currencyDataList = signalCurrencyApiResult?.Data.InnerData?.Where(x => x.Id is 200000 or 200001).ToList();
        var goldDataList = signalGoldApiResult?.Data.InnerData?.Where(x => x.Id is >= 100010 and <= 100014).ToList();
        var coinDataList = signalCoinApiResult?.Data.InnerData?.Where(x => x.Id is >= 100000 and <= 100027).ToList();

        if (currencyDataList is null || goldDataList is null || coinDataList is null)
            return null;

        var priceApiResult = new PriceApiResult
        {
            UsDollar = currencyDataList.FirstOrDefault(x => x.Id == 200000)!.Close.RemoveLeadingZero(),
            UsEuro = currencyDataList.FirstOrDefault(x => x.Id == 200001)!.Close.RemoveLeadingZero()
        };

        foreach (var goldData in goldDataList)
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

        foreach (var coinData in coinDataList)
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
}