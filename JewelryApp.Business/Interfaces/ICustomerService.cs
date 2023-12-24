using ErrorOr;
using JewelryApp.Shared.Requests.Customer;
using JewelryApp.Shared.Responses.Customer;

namespace JewelryApp.Application.Interfaces;

public interface ICustomerService
{
    Task<ErrorOr<AddCustomerResponse>> AddCustomerAsync(AddCustomerRequest request, CancellationToken token = default);
    Task<ErrorOr<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken token = default);
    Task<ErrorOr<RemoveCustomerResponse>> RemoveCustomerAsync(int id, CancellationToken token = default);
    Task<ErrorOr<GetCustomerResponse>> GetCustomerByInvoiceIdAsync(int id, CancellationToken token = default);
    Task<ErrorOr<GetCustomerResponse>> GetCustomerByPhoneNumberAsync(string phoneNumber, CancellationToken token = default);
    Task<ErrorOr<GetCustomerResponse>> GetCustomerByIdAsync(int id, CancellationToken token = default);
}