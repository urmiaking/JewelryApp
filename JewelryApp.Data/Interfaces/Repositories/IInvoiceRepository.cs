using JewelryApp.Data.Interfaces.Repositories.Base;
using JewelryApp.Data.Models;

namespace JewelryApp.Data.Interfaces.Repositories;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<IQueryable<Invoice>?> GetAllInvoices(CancellationToken token = default);
}
