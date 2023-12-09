using ErrorOr;
using JewelryApp.Common.Enums;
using JewelryApp.Models.Dtos.InvoiceDtos;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Responses.Invoices;

namespace JewelryApp.Business.Interfaces;

public interface IInvoiceService
{
    Task<IEnumerable<GetInvoiceTableResponse>?> GetInvoicesAsync(GetInvoiceTableRequest request, CancellationToken cancellationToken = default);
    Task<GetInvoiceDetailsResponse?> GetInvoiceDetailsAsync(GetInvoiceDetailsRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<AddInvoiceResponse>> AddInvoiceAsync(AddInvoiceRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<UpdateInvoiceResponse>> UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<RemoveInvoiceResponse>> RemoveInvoiceAsync(RemoveInvoiceRequest request, CancellationToken cancellationToken = default);
    Task<bool> SetInvoiceAsync(InvoiceDto invoiceDto, CancellationToken cancellationToken);
    Task<DeleteResult> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<int> GetTotalInvoicesCount(CancellationToken cancellationToken);
    Task<bool> UpdateInvoiceHeaderAsync(InvoiceHeaderDto invoiceHeaderDto, CancellationToken cancellationToken);
}