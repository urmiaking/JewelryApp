using JewelryApp.Common.Enums;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;

namespace JewelryApp.Business.AppServices;

public interface IProductService
{
    Task<Product> SetProductAsync(SetProductDto productDto);
    Task<IEnumerable<ProductTableItemDto>> GetProductsAsync(int page, int pageSize, string sortDirection, string sortLabel, string searchString, CancellationToken cancellationToken);
    Task<DeleteResult> DeleteProductAsync(int id, CancellationToken cancellationToken);
    Task<int> GetTotalProductsCount(CancellationToken cancellationToken);
}