using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories.Base;

namespace JewelryApp.Core.Interfaces.Repositories;

public interface IPriceRepository : IRepository<Price>
{
    public Task<Price?> GetLastSavedPriceAsync(CancellationToken cancellationToken = default);
}