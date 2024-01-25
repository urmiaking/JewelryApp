using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories.Base;

namespace JewelryApp.Core.Interfaces.Repositories;

public interface IOldGoldRepository : IRepository<OldGold>
{
    Task<List<OldGold>> GetOldGoldsByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken = default);
}