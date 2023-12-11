using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

public class InvoiceItemRepository : RepositoryBase<InvoiceItem>, IInvoiceItemRepository
{
    public InvoiceItemRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) : base(dbContext, httpContextAccessor, userManager)
    {
    }

    public IQueryable<InvoiceItem> GetInvoiceItemsByInvoiceId(int invoiceId)
        => TableNoTracking.Where(x => x.InvoiceId == invoiceId);
}