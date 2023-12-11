using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) 
        : base(context, httpContextAccessor, userManager)
    {

    }

    public async Task<Product?> GetByNameAsync(string name, CancellationToken token = default)
        => await TableNoTracking
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase), token);

    public async Task<bool> CheckBarcodeExistsAsync(string barcode, CancellationToken token = default) => 
        await TableNoTracking.AnyAsync(x => x.Barcode == barcode, token);
        
    public async Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken token = default)
    {
        var userId = await GetUserIdAsync();

        if (userId is null)
            return null;

        if (!await IsAdminAsync())
            return await TableNoTracking.SingleOrDefaultAsync(x => x.Barcode == barcode && x.ModifiedUserId == userId, token);

        return await TableNoTracking.SingleOrDefaultAsync(x => x.Barcode == barcode, token);
    }
         

    public async Task<bool> CheckProductIsSoldAsync(int productId, CancellationToken token = default)
        => await TableNoTracking.AnyAsync(x => x.Id == productId && x.SellDateTime.HasValue);

    public async Task<int> GetProductsCountAsync(CancellationToken token = default)
    {
        var userId = await GetUserIdAsync();

        if (userId == null) 
            return 0;

        if (!await IsAdminAsync())
        {
            return await TableNoTracking.CountAsync(x => x.ModifiedUserId == userId, token);
        }

        return await TableNoTracking.CountAsync(token);
    }

    public async Task<IQueryable<Product>?> GetAllProductsAsync(CancellationToken token = default)
    {
        var userId = await GetUserIdAsync();

        if (userId is null)
            return null;
        
        if (!await IsAdminAsync())
        {
            return TableNoTracking.Where(x => x.ModifiedUserId == userId);
        }

        return TableNoTracking;
    }
}
