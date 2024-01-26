using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using JewelryApp.Shared.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

[ScopedService<IInvoiceItemRepository>]
public class InvoiceItemRepository : RepositoryBase<InvoiceItem>, IInvoiceItemRepository
{
    public InvoiceItemRepository(AppDbContext dbContext, IElevatedAccessService elevatedAccessService, UserManager<AppUser> userManager) : base(dbContext, elevatedAccessService, userManager)
    {
    }

    public IQueryable<InvoiceItem> GetInvoiceItemsByInvoiceId(int invoiceId)
        => Get(retrieveDeletedRecords: true).Where(x => x.InvoiceId == invoiceId);

    public async Task<bool> CheckInvoiceItemExistsAsync(int invoiceId, int productId, CancellationToken cancellationToken = default)
        => await Get().AnyAsync(x => x.InvoiceId == invoiceId && x.ProductId == productId, cancellationToken);

    public async Task<bool> CheckProductIsSoldAsync(int productId, CancellationToken token = default)
        => await Get(asNoTracking: false).AnyAsync(x => x.ProductId == productId, token);

    public IQueryable<int> GetSoldProductIds()
        => Get().Select(x => x.ProductId);
}