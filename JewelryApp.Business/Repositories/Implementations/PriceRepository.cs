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
            var latestPrice = await _context.GramPrices.OrderBy(a => a.UpdatedDateTime).LastOrDefaultAsync();

            var onlinePrice = await _apiPrice.GetGramPrice();

            if (latestPrice is null || (onlinePrice != 0 && latestPrice.Price != onlinePrice))
            {
                await AddGramPriceAsync(onlinePrice);
                return onlinePrice;
            }

            return onlinePrice;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<GramPrice> AddGramPriceAsync(double value)
    {
        var gramPrice = new GramPrice
        {
            Price = value,
            UpdatedDateTime = DateTime.Now,
        };

        await _context.GramPrices.AddAsync(gramPrice);
        await _context.SaveChangesAsync();

        return gramPrice;
    }
}