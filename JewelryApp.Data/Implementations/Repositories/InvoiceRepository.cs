using JewelryApp.Data.Implementations.Repositories.Base;
using JewelryApp.Data.Interfaces.Repositories;
using JewelryApp.Data.Models;
using JewelryApp.Data.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Implementations.Repositories;

public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, AppDbContext context)
        : base(context, httpContextAccessor, userManager)
    {
        
    }

    public async Task<IQueryable<Invoice>?> GetAllInvoices(CancellationToken token = default)
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
