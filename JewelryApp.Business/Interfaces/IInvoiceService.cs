using ErrorOr;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Responses.Invoices;

namespace JewelryApp.Application.Interfaces;

public interface IInvoiceService
{
    Task<IEnumerable<GetInvoiceTableResponse>?> GetInvoicesAsync(GetInvoiceTableRequest request, CancellationToken cancellationToken = default);
    Task<GetInvoiceResponse?> GetInvoiceAsync(GetInvoiceRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<AddInvoiceResponse>> AddInvoiceAsync(AddInvoiceRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<UpdateInvoiceResponse>> UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<RemoveInvoiceResponse>> RemoveInvoiceAsync(RemoveInvoiceRequest request, CancellationToken cancellationToken = default);
    Task<int> GetTotalInvoicesCount(CancellationToken cancellationToken);
}