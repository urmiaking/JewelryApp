using JewelryApp.Core.Attributes;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

[ScopedService<IPriceRepository>]
public class PriceRepository : RepositoryBase<Price>, IPriceRepository
{
    public PriceRepository(AppDbContext dbContext, IElevatedAccessService elevatedAccessService, UserManager<AppUser> userManager) 
        : base(dbContext, elevatedAccessService, userManager)
    {

    }

    public async Task<Price?> GetLastSavedPriceAsync(CancellationToken cancellationToken = default)
        => await Get().OrderByDescending(x => x.DateTime).FirstOrDefaultAsync(cancellationToken);
}