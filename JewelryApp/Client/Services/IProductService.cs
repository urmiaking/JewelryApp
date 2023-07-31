using JewelryApp.Models.Dtos;

namespace JewelryApp.Client.Services;

public interface IProductService
{
    Task<IEnumerable<ProductTableItemDto>?> GetProductsAsync();

    Task<bool> AddOrEditProductAsync(SetProductDto productDto);

    Task<bool> DeleteProductAsync(int productId);
}