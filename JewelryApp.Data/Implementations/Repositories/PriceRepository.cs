using JewelryApp.Data.Interfaces.Repositories;
using JewelryApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Data.Implementations.Repositories;

public class PriceRepository : RepositoryBase<Price>, IPriceRepository
{
    public PriceRepository(AppDbContext dbContext) : base(dbContext)
    {

    }

    public async Task<Price?> GetLastSavedPriceAsync(CancellationToken cancellationToken = default)
    {
        var lastSavedPrice =
            await TableNoTracking.OrderByDescending(x => x.DateTime).FirstOrDefaultAsync(cancellationToken);

        return lastSavedPrice;
    }
}