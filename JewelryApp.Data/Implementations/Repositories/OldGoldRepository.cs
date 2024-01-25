using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Infrastructure.Implementations.Repositories.Base;
using JewelryApp.Shared.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Infrastructure.Implementations.Repositories;

[ScopedService<IOldGoldRepository>]
public class OldGoldRepository : RepositoryBase<OldGold>, IOldGoldRepository
{
    public OldGoldRepository(AppDbContext dbContext, IElevatedAccessService elevatedAccessService, UserManager<AppUser> userManager) 
        : base(dbContext, elevatedAccessService, userManager)
    {
    }

    public async Task<List<OldGold>> GetOldGoldsByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken = default)
        => await Get().Where(x => x.InvoiceId == invoiceId).ToListAsync(cancellationToken);
    
}