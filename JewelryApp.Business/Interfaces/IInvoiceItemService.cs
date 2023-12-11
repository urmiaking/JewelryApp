using ErrorOr;
using JewelryApp.Shared.Requests.InvoiceItems;
using JewelryApp.Shared.Responses.InvoiceItems;

namespace JewelryApp.Application.Interfaces;

public interface IInvoiceItemService
{
    Task<IEnumerable<GetInvoiceItemResponse>> GetInvoiceItemsAsync(GetInvoiceItemsRequest request, CancellationToken  cancellationToken = default);
    Task<ErrorOr<AddInvoiceItemResponse>> AddInvoiceItemAsync(AddInvoiceItemRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<UpdateInvoiceItemResponse>> UpdateInvoiceItemAsync(UpdateInvoiceItemRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<RemoveInvoiceItemResponse>> RemoveInvoiceItemAsync(RemoveInvoiceItemRequest  request, CancellationToken cancellationToken = default);
}