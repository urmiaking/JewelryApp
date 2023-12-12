using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories.Base;

namespace JewelryApp.Core.Interfaces.Repositories;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<IQueryable<Invoice>?> GetAllInvoices(CancellationToken token = default);
    Task<int> GetTotalInvoicesCount(CancellationToken cancellationToken = default);
}
