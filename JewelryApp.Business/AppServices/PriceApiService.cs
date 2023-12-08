using System.Net.Http.Json;
using JewelryApp.Common.Constants;
using JewelryApp.Common.Utilities;
using JewelryApp.Business.Interfaces;
using JewelryApp.Business.ExternalModels.Signal;

namespace BlazingJewelry.Application.Services;

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

        if (!signalCurrencyResponse.IsSuccessStatusCode || !signalGoldResponse.IsSuccessStatusCode)
            return null;

        var signalCurrencyApiResult = await signalCurrencyResponse.Content.ReadFromJsonAsync<SignalApiResult>(cancellationToken: token);

        var signalGoldApiResult = await signalGoldResponse.Content.ReadFromJsonAsync<SignalApiResult>(cancellationToken: token);

        var currencyData = signalCurrencyApiResult?.Data.InnerData?.FirstOrDefault(x => x.Id == 200000);
        var goldDataList = signalGoldApiResult?.Data.InnerData?.Where(x => x.Id is >= 100010 and <= 100014);

        if (currencyData is null || goldDataList is null)
            return null;

        var priceApiResult = new PriceApiResult
        {
            UsDollar = currencyData.Close.RemoveLeadingZero()
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

        return priceApiResult;
    }
}