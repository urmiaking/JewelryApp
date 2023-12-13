using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories.Base;

namespace JewelryApp.Core.Interfaces.Repositories;

public interface IProductCategoryRepository : IRepository<ProductCategory>
{
    Task<ProductCategory?> FindByNameAsync(string name, CancellationToken token = default);

    Task<bool> CheckExistenceAsync(string name, CancellationToken token = default);

    Task<bool> CheckUsedAsync(int id, CancellationToken token = default);
}