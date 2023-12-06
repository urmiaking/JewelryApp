using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using JewelryApp.Models.Dtos.PriceDtos.Signal;
using JewelryApp.Common.Constants;
using JewelryApp.Common.Utilities;
using JewelryApp.Business.Interfaces;

namespace BlazingJewelry.Application.Services;

public class PriceApiService : IPriceApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PriceApiService> _logger;

    public PriceApiService(HttpClient httpClient, ILogger<PriceApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PriceApiResult?> GetPriceAsync(CancellationToken token)
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
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }
}