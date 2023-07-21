using JewelryApp.Models.Dtos;

namespace JewelryApp.Client.Services;

public interface IProductService
{
    Task<IEnumerable<ProductTableItemDto>?> GetProductsAsync(int count = 0);
}