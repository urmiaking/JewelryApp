using AutoMapper;
using JewelryApp.Business.Interfaces;
using JewelryApp.Data.Interfaces.Repositories;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.PriceDtos;
using JewelryApp.Models.Dtos.PriceDtos.Signal;

namespace BlazingJewelry.Application.Services;

public class PriceService : IPriceService
{
    private readonly IMapper _mapper;
    private readonly IPriceApiService _priceApiService;
    private readonly IPriceRepository _priceRepository;

    public PriceService(IMapper mapper, IPriceApiService priceApiService, IPriceRepository priceRepository)
    {
        _mapper = mapper;
        _priceApiService = priceApiService;
        _priceRepository = priceRepository;
    }

    public async Task<PriceDto> GetPriceAsync(CancellationToken cancellationToken = default)
    {
        var latestPrice = await _priceApiService.GetPriceAsync(cancellationToken);

        var price = _mapper.Map<PriceApiResult?, Price>(latestPrice);

        var latestSavedPrice = await _priceRepository.GetLastSavedPriceAsync(cancellationToken);

        if (latestSavedPrice is null || !ArePricesIdentical(price, latestSavedPrice))
        {
            price.DateTime = DateTime.Now;

            await _priceRepository.AddAsync(price, cancellationToken);
        }

        var priceDto = _mapper.Map<Price, PriceDto>(price);

        return priceDto;
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