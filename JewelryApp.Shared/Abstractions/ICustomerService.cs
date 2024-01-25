using ErrorOr;
using JewelryApp.Shared.Requests.Customer;
using JewelryApp.Shared.Responses.Customer;

namespace JewelryApp.Shared.Abstractions;

public interface ICustomerService
{
    Task<ErrorOr<AddCustomerResponse>> AddCustomerAsync(AddCustomerRequest request, CancellationToken token = default);
    Task<ErrorOr<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken token = default);
    Task<ErrorOr<RemoveCustomerResponse>> RemoveCustomerAsync(int id, bool deletePermanently = false, CancellationToken token = default);
    Task<ErrorOr<GetCustomerResponse>> GetCustomerByInvoiceIdAsync(int id, CancellationToken token = default);
    Task<ErrorOr<GetCustomerResponse>> GetCustomerByPhoneNumberAsync(string phoneNumber, CancellationToken token = default);
    Task<ErrorOr<GetCustomerResponse>> GetCustomerByNationalCodeAsync(string nationalCode, CancellationToken token = default);
    Task<ErrorOr<GetCustomerResponse>> GetCustomerByIdAsync(int id, CancellationToken token = default);
}