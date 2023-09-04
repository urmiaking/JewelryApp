using AutoMapper;
using HtmlAgilityPack;
using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Common.DateFunctions;
using JewelryApp.Common.Enums;
using JewelryApp.Data;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.Repositories.Implementations;

public class PriceRepository : IPriceRepository
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public PriceRepository(HttpClient httpClient, AppDbContext context, IMapper mapper)
    {
        _httpClient = httpClient;
        _context = context;
        _mapper = mapper;
    }

    public async Task<PriceDto> GetPriceAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://www.tala.ir/webservice/price_live.php");

            if (!response.IsSuccessStatusCode)
                return null;

            var htmlResponse = await response.Content.ReadAsStringAsync();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlResponse);

            var gold18KTag = htmlDocument.GetElementbyId("gold_18k");
            var gold24KTag = htmlDocument.GetElementbyId("gold_24k");
            var goldOunceTag = htmlDocument.GetElementbyId("gold_ounce");
            var oldCoinTag = htmlDocument.GetElementbyId("sekke-gad");
            var newCoinTag = htmlDocument.GetElementbyId("sekke-jad");
            var halfCoinTag = htmlDocument.GetElementbyId("sekke-nim");
            var quarterCoinTag = htmlDocument.GetElementbyId("sekke-rob");
            var gramCoinTag = htmlDocument.GetElementbyId("sekke-grm");

            _httpClient.Dispose();

            var priceModel = new PriceDto
            {
                Gold18K = double.Parse(gold18KTag.InnerHtml.Replace(",", "")),
                Gold24K = double.Parse(gold24KTag.InnerHtml.Replace(",", "")),
                GoldOunce = double.Parse(goldOunceTag.InnerHtml.Replace(",", "")),
                OldCoin = double.Parse(oldCoinTag.InnerHtml.Replace(",", "")),
                NewCoin = double.Parse(newCoinTag.InnerHtml.Replace(",", "")),
                HalfCoin = double.Parse(halfCoinTag.InnerHtml.Replace(",", "")),
                QuarterCoin = double.Parse(quarterCoinTag.InnerHtml.Replace(",", "")),
                GramCoin = double.Parse(gramCoinTag.InnerHtml.Replace(",", "")),
            };

            return priceModel;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task AddPriceAsync(PriceDto priceDto)
    {
        var prevPrice = await _context.Prices.OrderByDescending(a => a.DateTime).FirstOrDefaultAsync();

        var price = _mapper.Map<PriceDto, Price>(priceDto);

        if (prevPrice is not null && ArePricesIdentical(prevPrice, price))
            return;

        price.DateTime = DateTime.Now;

        _context.Prices.Add(price);
        await _context.SaveChangesAsync();
    }

    public async Task<LineChartDto> GetCaretChartDataAsync(CaretChartType caretChartType)
    {
        var result = new LineChartDto();


        var today = DateTime.Today.AddHours(23);

        DateTime startOfWeek;

        switch (caretChartType)
        {
            case CaretChartType.Weekly:
                //Filter by days

                startOfWeek = today.AddDays(-7);

                var prices = await _context.Prices
                    .Where(p => p.DateTime >= startOfWeek && p.DateTime <= today)
                    .GroupBy(p => p.DateTime.Value.Date)
                    .Select(p => new
                    {
                        p.Key,
                        gold18k = p.Max(x => x.Gold18K),
                        gold24k = p.Max(x => x.Gold24K)
                    })
                    .OrderBy(p => p.Key)
                    .ToListAsync();

                result.XAxisValues = prices.Select(a => a.Key.ToShamsiDateString()).ToArray();
                result.Data = new List<Line>
                {
                    new() { Name = "طلای 18 عیار", Data = prices.Select(a => a.gold18k).ToArray() },
                    new() { Name = "طلای 24 عیار", Data = prices.Select(a => a.gold24k).ToArray() }
                };

                break;
            case CaretChartType.Monthly:
                //Filter by weeks

                //var oneMonthAgo = today.AddMonths(-1);

                //var startingDate = today.Date.AddHours(23);

                //var weeklyPrices = _context.Prices
                //    .Where(p => p.DateTime >= oneMonthAgo && p.DateTime <= startingDate) // Filter the data for the past month
                //    .GroupBy(p => new { WeekNumber = EF.Functions.DatePartIsoWeek(p.DateTime), Year = p.DateTime.Value.Year }) // Group by week number and year
                //    .Select(g => new
                //    {
                //        Week = g.Key.WeekNumber,
                //        Year = g.Key.Year,
                //        Gold18K = g.Min(p => p.Gold18K), // Get the minimum value of Gold18K for each week
                //        Gold24K = g.Min(p => p.Gold24K)  // Get the minimum value of Gold24K for each week
                //    })
                //    .ToList();

                //result.XAxisValues = monthlyPrices.Select(a => a.Key.ToShamsiDateString()).ToArray();
                //result.Data = new List<Line>
                //{
                //    new() { Name = "طلای 18 عیار", Data = monthlyPrices.Select(a => a.gold18k).ToArray() },
                //    new() { Name = "طلای 24 عیار", Data = monthlyPrices.Select(a => a.gold24k).ToArray() }
                //};

                break;
            case CaretChartType.Yearly:
                //Filter by month
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(caretChartType), caretChartType, null);
        }

        return result;
    }

    private static bool ArePricesIdentical(Price price1, Price price2)
    {
        return price1.Gold18K == price2.Gold18K &&
               price1.Gold24K == price2.Gold24K &&
               price1.GoldOunce == price2.GoldOunce &&
               price1.OldCoin == price2.OldCoin &&
               price1.NewCoin == price2.NewCoin &&
               price1.HalfCoin == price2.HalfCoin &&
               price1.QuarterCoin == price2.QuarterCoin &&
               price1.GramCoin == price2.GramCoin;
    }
}