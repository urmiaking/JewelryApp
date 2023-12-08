using JewelryApp.Data.Interfaces.Repositories.Base;
using JewelryApp.Data.Models;

namespace JewelryApp.Data.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByNameAsync(string name, CancellationToken token = default);
    Task<bool> CheckBarcodeExistsAsync(string barcode, CancellationToken token = default);
    Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken token = default);
    Task<bool> CheckProductIsSoldAsync(int productId, CancellationToken token = default);
    Task<int> GetProductsCountAsync(CancellationToken token = default);
    Task<IQueryable<Product>?> GetAllProductsAsync(CancellationToken token = default);
}
