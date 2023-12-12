using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

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

    public async Task<int> GetTotalInvoicesCount(CancellationToken token = default)
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
}
