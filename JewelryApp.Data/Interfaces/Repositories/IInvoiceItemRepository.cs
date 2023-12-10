using JewelryApp.Data.Interfaces.Repositories.Base;
using JewelryApp.Data.Models;

namespace JewelryApp.Data.Interfaces.Repositories;

public interface IInvoiceItemRepository : IRepository<InvoiceItem>
{
    IQueryable<InvoiceItem> GetInvoiceItemsByInvoiceId(int invoiceId);
}