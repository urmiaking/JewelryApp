using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Common.DateFunctions;
using JewelryApp.Data;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.Repositories.Implementations;

public class PriceRepository : IPriceRepository
{
    private readonly IApiPrice _apiPrice;
    private readonly AppDbContext _context;

    public PriceRepository(IApiPrice apiPrice, AppDbContext context)
    {
        _apiPrice = apiPrice;
        _context = context;
    }

    public async Task<double> GetGramPrice()
    {
        try
        {
            var latestPrice = await _context.GramPrices.OrderBy(a => a.RequestDateTime).LastOrDefaultAsync();

            if (latestPrice is null)
            {
                var item = await _apiPrice.GetGramPrice();

                if (item is not null)
                {
                    var gramPrice = await AddGramPriceAsync(item);

                    return gramPrice.Price;
                }

                return 0;
            }

            if (latestPrice.RequestDateTime <= DateTime.Now.AddHours(-2))
            {
                var item = await _apiPrice.GetGramPrice();

                if (item is null)
                    return 0;

                var gramPrice = await AddGramPriceAsync(item);

                return gramPrice.Price;
            }

            return latestPrice.Price;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<string> GetLastUpdateTime()
    {
        var gramPrice = await _context.GramPrices.OrderBy(a => a.RequestDateTime).LastOrDefaultAsync();

        if (gramPrice is not null)
        {
            return gramPrice.UpdatedDateTimeString.ExtractTime();
        }

        var item = await _apiPrice.GetGramPrice();

        if (item is null)
            return "";

        var gramPriceOnline = await AddGramPriceAsync(item);

        return gramPriceOnline.UpdatedDateTimeString.ExtractTime();

    }

    public async Task<GramPrice> AddGramPriceAsync(Item item)
    {
        var gramPrice = new GramPrice
        {
            Price = double.Parse(item.Value),
            Change = item.Change,
            RequestDateTime = DateTime.Now,
            UpdatedDateTime = ShamsiDateTime.ParseShamsiDateTime(item.Date),
            UpdatedDateTimeString = item.Date
        };

        await _context.GramPrices.AddAsync(gramPrice);
        await _context.SaveChangesAsync();

        return gramPrice;
    }
}