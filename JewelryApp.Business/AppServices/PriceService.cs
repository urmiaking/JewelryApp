using AutoMapper;
using JewelryApp.Application.ExternalApis.Abstraction;
using JewelryApp.Application.ExternalModels.Signal;
using JewelryApp.Application.Interfaces;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Responses.Prices;
using Microsoft.Extensions.Logging;
using static JewelryApp.Shared.Errors.Errors;

namespace JewelryApp.Application.AppServices;

[ScopedService<IPriceService>]
public class PriceService : IPriceService
{
    private readonly IMapper _mapper;
    private readonly ILogger<PriceService> _logger;
    private readonly ICoinService _coinService;
    private readonly IGoldService _goldService;
    private readonly ICurrencyService _currencyService;
    private readonly IPriceRepository _priceRepository;

    public PriceService(IMapper mapper, ICoinService coinService, IPriceRepository priceRepository, ILogger<PriceService> logger,
        IGoldService goldService, ICurrencyService currencyService)
    {
        _mapper = mapper;
        _logger = logger;
        _goldService = goldService;
        _currencyService = currencyService;
        _coinService = coinService;
        _priceRepository = priceRepository;
    }

    public async Task<PriceResponse?> GetPriceAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Price currencyPrice;
            Price goldPrice;
            Price coinPrice;

            do
            {
                var currencyResult = await _currencyService.GetCurrencyAsync(cancellationToken);

                currencyPrice = _mapper.Map<PriceApiResult, Price>(currencyResult);

            } while (currencyPrice.UsDollar == 0);

            do
            {
                var goldResult = await _goldService.GetGoldPriceAsync(cancellationToken);

                goldPrice = _mapper.Map<PriceApiResult, Price>(goldResult);

            } while (goldPrice.Gram18 == 0);

            do
            {
                var coinResult = await _coinService.GetCoinPriceAsync(cancellationToken);

                coinPrice = _mapper.Map<PriceApiResult, Price>(coinResult);

            } while (coinPrice.CoinBahar == 0);

            var price = new Price
            {
                Gram17 = goldPrice.Gram17,
                Gram18 = goldPrice.Gram18,
                Gram24 = goldPrice.Gram24,
                Mazanneh = goldPrice.Mazanneh,
                Mesghal = goldPrice.Mesghal,
                UsDollar = currencyPrice.UsDollar,
                UsEuro = currencyPrice.UsEuro,
                CoinImam = coinPrice.CoinImam,
                CoinNim = coinPrice.CoinNim,
                CoinRob = coinPrice.CoinRob,
                CoinBahar = coinPrice.CoinBahar,
                CoinGrami = coinPrice.CoinGrami,
                CoinParsian500Sowt = coinPrice.CoinParsian500Sowt,
                CoinParsian400Sowt = coinPrice.CoinParsian400Sowt,
                CoinParsian300Sowt = coinPrice.CoinParsian300Sowt,
                CoinParsian250Sowt = coinPrice.CoinParsian250Sowt,
                CoinParsian200Sowt = coinPrice.CoinParsian200Sowt,
                CoinParsian150Sowt = coinPrice.CoinParsian150Sowt,
                CoinParsian100Sowt = coinPrice.CoinParsian100Sowt,
                CoinParsian50Sowt = coinPrice.CoinParsian50Sowt,
                CoinParsian1Gram = coinPrice.CoinParsian1Gram,
                CoinParsian2Gram = coinPrice.CoinParsian2Gram,
                CoinParsian15Gram = coinPrice.CoinParsian15Gram,
                DateTime = coinPrice.DateTime
            };

            var latestSavedPrice = await _priceRepository.GetLastSavedPriceAsync(cancellationToken);

            if (latestSavedPrice is null)
            {
                await _priceRepository.AddAsync(price, cancellationToken);
            }
            else
            {
                if (!IsPricesIdentical(price, latestSavedPrice))
                {
                    await _priceRepository.AddAsync(price, cancellationToken);
                }
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

    private static bool IsPricesIdentical(Price price1, Price price2)
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