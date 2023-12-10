using JewelryApp.Data.Implementations.Repositories.Base;
using JewelryApp.Data.Interfaces.Repositories;
using JewelryApp.Data.Models;
using JewelryApp.Data.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Implementations.Repositories;

public class InvoiceItemRepository : RepositoryBase<InvoiceItem>, IInvoiceItemRepository
{
    public InvoiceItemRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) : base(dbContext, httpContextAccessor, userManager)
    {
    }

    public IQueryable<InvoiceItem> GetInvoiceItemsByInvoiceId(int invoiceId)
        => TableNoTracking.Where(x => x.InvoiceId == invoiceId);
}