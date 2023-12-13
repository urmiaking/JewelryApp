using AutoMapper;
using ErrorOr;
using JewelryApp.Application.Interfaces;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Errors;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Core.Interfaces.Repositories.Base;
using JewelryApp.Shared.Requests.Customer;
using JewelryApp.Shared.Responses.Customer;

namespace JewelryApp.Application.AppServices;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;

    public CustomerService(IRepository<Customer> customerRepository, IMapper mapper, IInvoiceRepository invoiceRepository)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<ErrorOr<AddCustomerResponse>> AddCustomerAsync(AddCustomerRequest request, CancellationToken token = default)
    {
        var customer = _mapper.Map<Customer>(request);

        await _customerRepository.AddAsync(customer, token);

        return new AddCustomerResponse(customer.Id);
    }

    public async Task<ErrorOr<GetCustomerResponse>> GetCustomerByInvoiceIdAsync(GetCustomerRequest request, CancellationToken token = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.Id, token);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        if (invoice.Customer is null)
            return Errors.Customer.NotFound;

        await _invoiceRepository.LoadReferenceAsync(invoice, x => x.Customer, token);

        var response = _mapper.Map<GetCustomerResponse>(invoice.Customer);

        return response;
    }

    public async Task<ErrorOr<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken token = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId, token);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        if (invoice.Customer is null)
            return Errors.Customer.NotFound;

        await _invoiceRepository.LoadReferenceAsync(invoice, x => x.Customer, token);

        invoice.Customer = _mapper.Map<Customer>(request);

        await _customerRepository.UpdateAsync(invoice.Customer, token);

        return new UpdateCustomerResponse(invoice.CustomerId);
    }

    public async Task<ErrorOr<RemoveCustomerResponse>> RemoveCustomerAsync(RemoveCustomerRequest request, CancellationToken token = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId, token);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        if (invoice.Customer is null)
            return Errors.Customer.NotFound;

        await _invoiceRepository.LoadReferenceAsync(invoice, x => x.Customer!, token);

        await _customerRepository.DeleteAsync(invoice.Customer, token);

        return new RemoveCustomerResponse(invoice.CustomerId);
    }
}