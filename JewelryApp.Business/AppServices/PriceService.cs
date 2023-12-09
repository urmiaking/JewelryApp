using AutoMapper;
using ErrorOr;
using JewelryApp.Business.Interfaces;
using JewelryApp.Common.Errors;
using JewelryApp.Data.Interfaces.Repositories;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.PriceDtos;
using JewelryApp.Models.Dtos.PriceDtos.Signal;
using JewelryApp.Shared.Responses.Prices;
using Microsoft.Extensions.Logging;

namespace JewelryApp.Business.AppServices;

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

    public async Task<ErrorOr<PriceResponse?>> GetPriceAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var latestPrice = await _priceApiService.GetPriceAsync(cancellationToken);

            if (latestPrice is null)
                return Errors.General.NoInternet;

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
            return Errors.General.ServerError;
        }

        
    }

    private static bool ArePricesIdentical(Price price1, Price price2)
    {
        return price1.Gram18 == price2.Gram18 &&
               price1.Gram17 == price2.Gram17 &&
               price1.Gram24 == price2.Gram24 &&
               price1.Mazanneh == price2.Mazanneh &&
               price1.Mesghal == price2.Mesghal &&
               price1.UsDollar == price2.UsDollar;
    }
}