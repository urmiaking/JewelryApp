using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using JewelryApp.Shared.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

[ScopedService<IProductRepository>]
public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    private readonly IInvoiceItemRepository _invoiceItemRepository;
    public ProductRepository(AppDbContext context, IElevatedAccessService elevatedAccessService, UserManager<AppUser> userManager, IInvoiceItemRepository invoiceItemRepository) 
        : base(context, elevatedAccessService, userManager)
    {
        _invoiceItemRepository = invoiceItemRepository;
    }

    public async Task<bool> CheckBarcodeExistsAsync(string barcode, CancellationToken token = default) => 
        await Get().AnyAsync(x => x.Barcode == barcode, token);
        
    public async Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken token = default)
        => await Get().FirstOrDefaultAsync(x => x.Barcode == barcode, token);

    public async Task<int> GetProductsCountAsync(CancellationToken token = default)
        => await Get(retrieveDeletedRecords: true).CountAsync(token);

    public IQueryable<Product> GetProductsInStock()
    {
        return Get().Where(p => !_invoiceItemRepository.GetSoldProductIds().Contains(p.Id));
    }
}
