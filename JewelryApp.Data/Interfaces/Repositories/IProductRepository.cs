using JewelryApp.Data.Interfaces.Repositories.Base;
using JewelryApp.Data.Models;

namespace JewelryApp.Data.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByNameAsync(string name, CancellationToken token = default);
}
