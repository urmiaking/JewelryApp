using JewelryApp.Models.Dtos;
using System.Net.Http.Json;

namespace JewelryApp.Client.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ProductTableItemDto>?> GetProductsAsync(int count = 0)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ProductTableItemDto>>($"/api/Products/GetProducts?count={count}");
        }
        catch
        {
            return new List<ProductTableItemDto>();
        }
    }
}