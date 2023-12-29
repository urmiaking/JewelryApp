using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using JewelryApp.Shared.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

[ScopedService<IProductCategoryRepository>]
public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
{
    private readonly IProductRepository _productRepository;
    public ProductCategoryRepository(AppDbContext dbContext, IElevatedAccessService elevatedAccessService, UserManager<AppUser> userManager, IProductRepository productRepository) 
        : base(dbContext, elevatedAccessService, userManager)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductCategory?> FindByNameAsync(string name, CancellationToken token = default)
        => await Get().SingleOrDefaultAsync(x => x.Name.Equals(name), token);

    public async Task<bool> CheckExistenceAsync(string name, CancellationToken token = default)
        => await Get().AnyAsync(x => x.Name.Equals(name), token);

    public async Task<bool> CheckUsedAsync(int id, CancellationToken token = default)
        => await _productRepository.Get().AnyAsync(x => x.ProductCategoryId == id, token);
}