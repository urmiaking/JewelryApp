using JewelryApp.Common.Enums;
using JewelryApp.Models.Dtos.InvoiceDtos;

namespace JewelryApp.Business.Interfaces;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceTableItemDto>> GetInvoicesAsync(int page, int pageSize, string sortDirection, string sortLabel, string searchString, CancellationToken cancellationToken);
    Task<InvoiceDto?> GetInvoiceAsync(int id, CancellationToken cancellationToken);
    Task<bool> SetInvoiceAsync(InvoiceDto invoiceDto, CancellationToken cancellationToken);
    Task<DeleteResult> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<int> GetTotalInvoicesCount(CancellationToken cancellationToken);
    Task<bool> UpdateInvoiceHeaderAsync(InvoiceHeaderDto invoiceHeaderDto, CancellationToken cancellationToken);
}