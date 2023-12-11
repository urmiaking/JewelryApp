using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories.Base;

namespace JewelryApp.Core.Interfaces.Repositories;

public interface IInvoiceItemRepository : IRepository<InvoiceItem>
{
    IQueryable<InvoiceItem> GetInvoiceItemsByInvoiceId(int invoiceId);
}