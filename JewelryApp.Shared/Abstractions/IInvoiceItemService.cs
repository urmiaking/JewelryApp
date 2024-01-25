using ErrorOr;
using JewelryApp.Shared.Requests.InvoiceItems;
using JewelryApp.Shared.Responses.InvoiceItems;

namespace JewelryApp.Shared.Abstractions;

public interface IInvoiceItemService
{
    Task<ErrorOr<GetInvoiceItemResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorOr<IEnumerable<GetInvoiceItemResponse>>> GetInvoiceItemsByInvoiceIdAsync(int invoiceId, CancellationToken  cancellationToken = default);
    Task<ErrorOr<AddInvoiceItemResponse>> AddInvoiceItemAsync(AddInvoiceItemRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<UpdateInvoiceItemResponse>> UpdateInvoiceItemAsync(UpdateInvoiceItemRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<RemoveInvoiceItemResponse>> RemoveInvoiceItemAsync(int id, bool deletePermanently = false, CancellationToken cancellationToken = default);
}