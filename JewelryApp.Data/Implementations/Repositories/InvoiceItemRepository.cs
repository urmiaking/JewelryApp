using JewelryApp.Core.Attributes;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

[ScopedService<IInvoiceItemRepository>]
public class InvoiceItemRepository : RepositoryBase<InvoiceItem>, IInvoiceItemRepository
{
    public InvoiceItemRepository(AppDbContext dbContext, IElevatedAccessService elevatedAccessService, UserManager<AppUser> userManager) : base(dbContext, elevatedAccessService, userManager)
    {
    }

    public IQueryable<InvoiceItem> GetInvoiceItemsByInvoiceId(int invoiceId)
        => Get(retrieveDeletedRecords: true).Where(x => x.InvoiceId == invoiceId);
}