using JewelryApp.Business.Repositories.Interfaces;
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
        _httpClient.BaseAddress = new Uri("http://api.navasan.tech/");
    }

    public async Task<Item> GetGramPrice()
    {
        try
        {
            var apiKey = await _db.ApiKeys.OrderByDescending(a => a.AddDateTime).FirstOrDefaultAsync(a => a.IsActive);

            var response = await _httpClient.GetAsync($"/latest/?api_key={apiKey!.Key}");

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadAsStringAsync();

            var priceResult = JsonConvert.DeserializeObject<PriceResult>(result);

            _httpClient.Dispose();

            return priceResult?.The18Ayar;
        }
        catch
        {
            return null;
        }
    }
}