using HtmlAgilityPack;
using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Common.DateFunctions;
using JewelryApp.Data;
using JewelryApp.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace JewelryApp.Business.Repositories.Implementations;

public class ApiPrice : IApiPrice
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _db;

    public ApiPrice(HttpClient httpClient, AppDbContext db)
    {
        _httpClient = httpClient;
        _db = db;
    }

    public async Task<double> GetGramPrice()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://www.tala.ir/webservice/price_live.php");

            if (!response.IsSuccessStatusCode)
                return 0;

            var htmlResponse = await response.Content.ReadAsStringAsync();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlResponse);

            var aTag = htmlDocument.GetElementbyId("gold_18k");

            _httpClient.Dispose();

            if (aTag != null)
            {
                var content = aTag.InnerHtml.Replace(",", "");
                
                return double.Parse(content);
            }

            return 0;

        }
        catch
        {
            return 0;
        }
    }
}