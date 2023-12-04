using JewelryApp.Data.Interfaces.Repositories.Base;
using JewelryApp.Data.Models;

namespace JewelryApp.Data.Interfaces.Repositories;

public interface IPriceRepository : IRepository<Price>
{
    public Task<Price?> GetLastSavedPriceAsync(CancellationToken cancellationToken = default);
}