using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

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