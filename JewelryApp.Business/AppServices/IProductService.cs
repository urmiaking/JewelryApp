using JewelryApp.Common.Enums;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;

namespace JewelryApp.Business.AppServices;

public interface IProductService
{
    Task<Product> SetProductAsync(SetProductDto productDto);
    Task<IEnumerable<ProductTableItemDto>> GetProductsAsync();
    Task<DeleteResult> DeleteProductAsync(int id, CancellationToken cancellationToken);
}