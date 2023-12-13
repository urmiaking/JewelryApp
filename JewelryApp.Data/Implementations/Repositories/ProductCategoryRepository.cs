using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
{
    private readonly IProductRepository _productRepository;
    public ProductCategoryRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IProductRepository productRepository) : base(dbContext, httpContextAccessor, userManager)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductCategory?> FindByNameAsync(string name, CancellationToken token = default)
        => await Table.FirstOrDefaultAsync(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase), token);

    public async Task<bool> CheckExistenceAsync(string name, CancellationToken token = default)
        => await TableNoTracking.AnyAsync(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase), token);

    public async Task<bool> CheckUsedAsync(int id, CancellationToken token = default)
        => await _productRepository.TableNoTracking.AnyAsync(x => x.ProductCategoryId == id, token);
}