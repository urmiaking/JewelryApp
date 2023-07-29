using System.Net.Http.Json;
using JewelryApp.Models.Dtos;
using static System.Net.WebRequestMethods;

namespace JewelryApp.Client.Services;

public class PriceService : IPriceService
{
    private readonly HttpClient _httpClient;

    public PriceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PriceModel?> GetPriceModel()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<PriceModel>("/api/Price/GetPrice");
        }
        catch
        {
            return new PriceModel
            {
                Price = 0
            };
        }
    }
    
}