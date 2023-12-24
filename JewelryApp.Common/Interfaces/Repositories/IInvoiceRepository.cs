using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories.Base;

namespace JewelryApp.Core.Interfaces.Repositories;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<bool> CheckInvoiceExistsAsync(int invoiceNumber, CancellationToken cancellationToken = default);
}
