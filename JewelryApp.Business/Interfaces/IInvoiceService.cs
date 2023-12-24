using ErrorOr;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Responses.Invoices;

namespace JewelryApp.Application.Interfaces;

public interface IInvoiceService
{
    Task<IEnumerable<GetInvoiceListResponse>?> GetInvoicesAsync(GetInvoiceListRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<GetInvoiceResponse>> GetInvoiceByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorOr<AddInvoiceResponse>> AddInvoiceAsync(AddInvoiceRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<UpdateInvoiceResponse>> UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<RemoveInvoiceResponse>> RemoveInvoiceAsync(int id, CancellationToken cancellationToken = default);
    Task<int> GetTotalInvoicesCount(CancellationToken cancellationToken);
}