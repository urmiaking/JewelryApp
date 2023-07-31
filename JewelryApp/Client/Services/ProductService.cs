using JewelryApp.Models.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace JewelryApp.Client.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ProductTableItemDto>?> GetProductsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ProductTableItemDto>>("/api/Products");
        }
        catch
        {
            return new List<ProductTableItemDto>();
        }
    }

    public async Task<bool> AddOrEditProductAsync(SetProductDto productDto)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync($"/api/Products", content);

            return result.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        try
        {
            var result = await _httpClient.DeleteAsync($"/api/Products/{productId}");

            return result.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}