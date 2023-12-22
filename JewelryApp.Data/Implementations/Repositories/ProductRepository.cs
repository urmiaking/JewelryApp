using JewelryApp.Core.Attributes;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

[ScopedService<IProductRepository>]
public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context, IElevatedAccessService elevatedAccessService, UserManager<AppUser> userManager) 
        : base(context, elevatedAccessService, userManager)
    {

    }

    public async Task<Product?> GetByNameAsync(string name, CancellationToken token = default)
        => await Get()
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.Name.Equals(name), token);

    public async Task<bool> CheckBarcodeExistsAsync(string barcode, CancellationToken token = default) => 
        await Get().AnyAsync(x => x.Barcode == barcode, token);
        
    public async Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken token = default)
        => await Get().FirstOrDefaultAsync(x => x.Barcode == barcode, token);

    public async Task<bool> CheckProductIsSoldAsync(int productId, CancellationToken token = default)
        => await Get().AnyAsync(x => x.Id == productId && x.SellDateTime.HasValue, token);

    public async Task<int> GetProductsCountAsync(CancellationToken token = default)
        => await Get().CountAsync(token);

    public IQueryable<Product> GetAll(CancellationToken token = default)
        => Get();
}
