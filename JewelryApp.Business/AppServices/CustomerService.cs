using AutoMapper;
using ErrorOr;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Errors;
using JewelryApp.Shared.Requests.Customer;
using JewelryApp.Shared.Responses.Customer;

namespace JewelryApp.Application.AppServices;

[ScopedService<ICustomerService>]
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper, IInvoiceRepository invoiceRepository)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<ErrorOr<AddCustomerResponse>> AddCustomerAsync(AddCustomerRequest request, CancellationToken token = default)
    {
        var customer = _mapper.Map<Customer>(request);

        var customerExists = await _customerRepository.CheckCustomerExistsAsync(customer, token);

        if (customerExists)
            return _mapper.Map<AddCustomerResponse>(await _customerRepository.GetByPhoneNumber(request.PhoneNumber, token));

        await _customerRepository.AddAsync(customer, token);

        return _mapper.Map<AddCustomerResponse>(customer);
    }

    public async Task<ErrorOr<GetCustomerResponse>> GetCustomerByInvoiceIdAsync(int id, CancellationToken token = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id, token);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        await _invoiceRepository.LoadReferenceAsync(invoice, x => x.Customer, token);

        var response = _mapper.Map<GetCustomerResponse>(invoice.Customer);

        return response;
    }

    public async Task<ErrorOr<GetCustomerResponse>> GetCustomerByPhoneNumberAsync(string phoneNumber, CancellationToken token = default)
    {
        var customer = await _customerRepository.GetByPhoneNumber(phoneNumber, token);

        if (customer is null)
            return Errors.Customer.NotFound;

        var response = _mapper.Map<GetCustomerResponse>(customer);

        return response;
    }

    public async Task<ErrorOr<GetCustomerResponse>> GetCustomerByNationalCodeAsync(string nationalCode, CancellationToken token = default)
    {
        var customer = await _customerRepository.GetByNationalCode(nationalCode, token);

        if (customer is null)
            return Errors.Customer.NotFound;

        var response = _mapper.Map<GetCustomerResponse>(customer);

        return response;
    }

    public async Task<ErrorOr<GetCustomerResponse>> GetCustomerByIdAsync(int id, CancellationToken token = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, token);

        if (customer is null)
            return Errors.Customer.NotFound;

        var response = _mapper.Map<GetCustomerResponse>(customer);

        return response;
    }

    public async Task<ErrorOr<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken token = default)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id, token);

        if (customer is null)
            return Errors.Customer.NotFound;
        
        customer = _mapper.Map<Customer>(request);

        await _customerRepository.UpdateAsync(customer, token);

        return new UpdateCustomerResponse(customer.Id);
    }

    public async Task<ErrorOr<RemoveCustomerResponse>> RemoveCustomerAsync(int id, bool deletePermanently = false, CancellationToken token = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, token);

        if (customer is null)
            return Errors.Customer.NotFound;

        if (customer.Deleted && !deletePermanently)
            return Errors.Customer.Deleted;

        await _customerRepository.DeleteAsync(customer, token, deletePermanently: deletePermanently);

        return new RemoveCustomerResponse(customer.Id);
    }
}