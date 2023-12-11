using ErrorOr;
using JewelryApp.Shared.Requests.Customer;
using JewelryApp.Shared.Responses.Customer;

namespace JewelryApp.Application.Interfaces;

public interface ICustomerService
{
    Task<ErrorOr<AddCustomerResponse>> AddCustomerAsync(AddCustomerRequest request, CancellationToken token = default);
    Task<ErrorOr<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken token = default);
    Task<ErrorOr<RemoveCustomerResponse>> RemoveCustomerAsync(RemoveCustomerRequest request, CancellationToken token = default);
    Task<ErrorOr<GetCustomerResponse>> GetCustomerByInvoiceIdAsync(GetCustomerRequest request, CancellationToken token = default);
}