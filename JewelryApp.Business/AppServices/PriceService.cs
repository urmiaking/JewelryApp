using AutoMapper;
using JewelryApp.Application.ExternalModels.Signal;
using JewelryApp.Application.Interfaces;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Responses.Prices;
using Microsoft.Extensions.Logging;

namespace JewelryApp.Application.AppServices;

[ScopedService<IPriceService>]
public class PriceService : IPriceService
{
    private readonly IMapper _mapper;
    private readonly ILogger<PriceService> _logger;
    private readonly IPriceApiService _priceApiService;
    private readonly IPriceRepository _priceRepository;

    public PriceService(IMapper mapper, IPriceApiService priceApiService, IPriceRepository priceRepository, ILogger<PriceService> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _priceApiService = priceApiService;
        _priceRepository = priceRepository;
    }

    public async Task<PriceResponse?> GetPriceAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var latestPrice = await _priceApiService.GetPriceAsync(cancellationToken);

            if (latestPrice is null)
                return null;

            var price = _mapper.Map<PriceApiResult?, Price>(latestPrice);

            var latestSavedPrice = await _priceRepository.GetLastSavedPriceAsync(cancellationToken);

            if (latestSavedPrice is null || !ArePricesIdentical(price, latestSavedPrice))
            {
                price.DateTime = DateTime.Now;

                await _priceRepository.AddAsync(price, cancellationToken);
            }

            var priceResponse = _mapper.Map<Price, PriceResponse>(price);

            return priceResponse;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }

        
    }

    private static bool ArePricesIdentical(Price price1, Price price2)
    {
        return price1.Gram18 == price2.Gram18 &&
               price1.Gram17 == price2.Gram17 &&
               price1.Gram24 == price2.Gram24 &&
               price1.Mazanneh == price2.Mazanneh &&
               price1.Mesghal == price2.Mesghal &&
               price1.UsDollar == price2.UsDollar &&
               price1.UsEuro == price2.UsEuro &&
               price1.CoinImam == price2.CoinImam &&
               price1.CoinNim == price2.CoinNim &&
               price1.CoinRob == price2.CoinRob &&
               price1.CoinBahar == price2.CoinBahar &&
               price1.CoinGrami == price2.CoinGrami &&
               price1.CoinParsian500Sowt == price2.CoinParsian500Sowt &&
               price1.CoinParsian400Sowt == price2.CoinParsian400Sowt &&
               price1.CoinParsian300Sowt == price2.CoinParsian300Sowt &&
               price1.CoinParsian250Sowt == price2.CoinParsian250Sowt &&
               price1.CoinParsian200Sowt == price2.CoinParsian200Sowt &&
               price1.CoinParsian150Sowt == price2.CoinParsian150Sowt &&
               price1.CoinParsian100Sowt == price2.CoinParsian100Sowt &&
               price1.CoinParsian50Sowt == price2.CoinParsian50Sowt &&
               price1.CoinParsian1Gram == price2.CoinParsian1Gram &&
               price1.CoinParsian2Gram == price2.CoinParsian2Gram &&
               price1.CoinParsian15Gram == price2.CoinParsian15Gram;
    }
}