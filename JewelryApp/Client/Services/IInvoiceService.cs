using JewelryApp.Models.Dtos;

namespace JewelryApp.Client.Services;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceTableItemDto>?> GetInvoicesAsync(int count = 0);
}