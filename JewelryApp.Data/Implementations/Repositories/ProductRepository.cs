using JewelryApp.Common.Utilities;
using JewelryApp.Data.Implementations.Repositories.Base;
using JewelryApp.Data.Interfaces.Repositories;
using JewelryApp.Data.Models;
using JewelryApp.Data.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Data.Implementations.Repositories;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) : base(context, httpContextAccessor, userManager)
    {

    }

    public async Task<Product?> GetByNameAsync(string name, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        var product = await TableNoTracking
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase), token);

        return product ?? null;
    }

    public override async Task AddAsync(Product entity, CancellationToken cancellationToken, bool saveNow = true)
    {
        var repeatedProduct = await TableNoTracking.AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .FirstOrDefaultAsync(a => a.Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase), cancellationToken);

        if (repeatedProduct is null)
            entity.Barcode = StringExtensions.GenerateBarcode();
        else
        {
            var existingBarcode = int.Parse(repeatedProduct.Barcode);
            existingBarcode += 1;
            entity.Barcode = existingBarcode.ToString();
        }

        await base.AddAsync(entity, cancellationToken, saveNow);
    }
}
