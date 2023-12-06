using JewelryApp.Data.Implementations.Repositories.Base;
using JewelryApp.Data.Interfaces.Repositories;
using JewelryApp.Data.Models;
using JewelryApp.Data.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Data.Implementations.Repositories;

public class PriceRepository : RepositoryBase<Price>, IPriceRepository
{
    public PriceRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) 
        : base(dbContext, httpContextAccessor, userManager)
    {

    }

    public async Task<Price?> GetLastSavedPriceAsync(CancellationToken cancellationToken = default)
    {
        var lastSavedPrice =
            await TableNoTracking.OrderByDescending(x => x.DateTime).FirstOrDefaultAsync(cancellationToken);

        return lastSavedPrice;
    }
}