using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories.Base;

namespace JewelryApp.Core.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByNameAsync(string name, CancellationToken token = default);
    Task<bool> CheckBarcodeExistsAsync(string barcode, CancellationToken token = default);
    Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken token = default);
    Task<bool> CheckProductIsSoldAsync(int productId, CancellationToken token = default);
    Task<int> GetProductsCountAsync(CancellationToken token = default);
    IQueryable<Product> GetAll(CancellationToken token = default);
}
