using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Models.Dtos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace JewelryApp.Business.Repositories.Implementations;

public class ApiPrice : IApiPrice
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ApiPrice(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://api.navasan.tech/");
        _apiKey = configuration["ApiKey"];
    }

    public async Task<Item> GetGramPrice()
    {
        try
        {
            var response = await _httpClient.GetAsync($"/latest/?api_key={_apiKey}");

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