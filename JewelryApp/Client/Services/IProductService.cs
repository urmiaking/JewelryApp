using JewelryApp.Models.Dtos;

namespace JewelryApp.Client.Services;

public interface IProductService
{
    Task<IEnumerable<ProductTableItemDto>?> GetProductsAsync(int count = 0);

    Task<bool> AddOrEditProductAsync(AddProductDto productDto);

    Task<bool> DeleteProductAsync(int productId);
}