using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErrorOr;
using JewelryApp.Application.Interfaces;
using JewelryApp.Core.Constants;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Responses.Invoices;
using Microsoft.EntityFrameworkCore;
using Errors = JewelryApp.Shared.Errors.Errors;

namespace JewelryApp.Application.AppServices;

[ScopedService<IInvoiceService>]
public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public InvoiceService(IMapper mapper,  IInvoiceRepository invoiceRepository, ICustomerRepository customerRepository)
    {
        _mapper = mapper;
        _invoiceRepository = invoiceRepository;
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<GetInvoiceListResponse>?> GetInvoicesAsync(GetInvoiceListRequest request, CancellationToken cancellationToken = default)
    {
        var invoices = _invoiceRepository.Get();

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            invoices = invoices.Where(a => a.Customer.FullName.Contains(request.SearchString) || (
                                                   !string.IsNullOrEmpty(a.Customer.PhoneNumber) &&
                                                   a.Customer.PhoneNumber.Contains(request.SearchString)));
        }

        if (!string.IsNullOrEmpty(request.SortLabel))
        {
            invoices = request.SortDirection switch
            {
                SortDirections.Ascending => invoices.OrderBy(p => GetPropertyValue(p, request.SortLabel)),
                SortDirections.Descending => invoices.OrderByDescending(p => GetPropertyValue(p, request.SortLabel)),
                _ => invoices
            };
        }

        var startIndex = request.Page * request.PageSize;
        invoices = invoices.Skip(startIndex).Take(request.PageSize);

        return await invoices
            .ProjectTo<GetInvoiceListResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<ErrorOr<GetInvoiceResponse>> GetInvoiceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        await _invoiceRepository.LoadReferenceAsync(invoice, x => x.Customer, cancellationToken);
        var response = _mapper.Map<GetInvoiceResponse>(invoice);

        return response;
    }

    public async Task<ErrorOr<AddInvoiceResponse>> AddInvoiceAsync(AddInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        var invoice = _mapper.Map<Invoice>(request);

        var invoiceExists = await _invoiceRepository.CheckInvoiceExistsAsync(request.InvoiceNumber, cancellationToken);

        if (invoiceExists)
            return Errors.Invoice.Exists;

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

        if (customer is null)
            return Errors.Customer.NotFound;      

        await _invoiceRepository.AddAsync(invoice, cancellationToken);

        var response = _mapper.Map<AddInvoiceResponse>(invoice);

        return response;
    }

    public async Task<ErrorOr<UpdateInvoiceResponse>> UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.Get(retrieveDeletedRecords: true)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        invoice = _mapper.Map<Invoice>(request);

        await _invoiceRepository.UpdateAsync(invoice, cancellationToken);

        return _mapper.Map<UpdateInvoiceResponse>(invoice);
    }

    public async Task<ErrorOr<RemoveInvoiceResponse>> RemoveInvoiceAsync(int id, bool deletePermanently = false, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        if (invoice.Deleted && !deletePermanently)
            return Errors.Invoice.Deleted;
        
        await _invoiceRepository.DeleteAsync(invoice, cancellationToken, deletePermanently: deletePermanently);

        return new RemoveInvoiceResponse(invoice.Id);
    }

    public async Task<GetInvoicesCountResponse> GetTotalInvoicesCount(CancellationToken cancellationToken = default)
    { 
        var count = await _invoiceRepository.Get().CountAsync(cancellationToken);

        return new GetInvoicesCountResponse(count);
    }

    public async Task<GetLastInvoiceNumberResponse> GetLastInvoiceNumber(CancellationToken cancellationToken = default)
    {
        var invoiceNumber = await _invoiceRepository.GetLastSavedInvoiceNumberAsync(cancellationToken);

        return new GetLastInvoiceNumberResponse(invoiceNumber);
    }

    private static object? GetPropertyValue(object obj, string propertyName)
        => obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
}