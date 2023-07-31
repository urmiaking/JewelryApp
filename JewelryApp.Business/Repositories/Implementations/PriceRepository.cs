using HtmlAgilityPack;
using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Models.Dtos;

namespace JewelryApp.Business.Repositories.Implementations;

public class PriceRepository : IPriceRepository
{
    private readonly HttpClient _httpClient;

    public PriceRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
}