using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErrorOr;
using JewelryApp.Business.Interfaces;
using JewelryApp.Common.Constants;
using JewelryApp.Common.Errors;
using JewelryApp.Data.Interfaces.Repositories;
using JewelryApp.Data.Interfaces.Repositories.Base;
using JewelryApp.Data.Models;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Responses.Invoices;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.AppServices;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IRepository<InvoiceItem> _invoiceItemRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IMapper _mapper;

    public InvoiceService(IMapper mapper, IInvoiceRepository invoiceRepository, IRepository<Product> productRepository, 
        IRepository<Customer> customerRepository, IRepository<InvoiceItem> invoiceItemRepository)
    {
        _mapper = mapper;
        _invoiceRepository = invoiceRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _invoiceItemRepository = invoiceItemRepository;
    }

    public async Task<IEnumerable<GetInvoiceTableResponse>?> GetInvoicesAsync(GetInvoiceTableRequest request, CancellationToken cancellationToken = default)
    {
        var invoices = await _invoiceRepository.GetAllInvoices(cancellationToken);

        if (invoices is null)
            return null;
        
        if (!string.IsNullOrEmpty(request.SearchString))
        {
            invoices = invoices.Where(a => a.Customer.FullName.Contains(request.SearchString, StringComparison.OrdinalIgnoreCase) ||
                                                   a.Customer.PhoneNumber.Contains(request.SearchString, StringComparison.OrdinalIgnoreCase));
        }

        invoices = request.SortDirection switch
        {
            SortDirections.Ascending => invoices.OrderBy(p => GetPropertyValue(p, request.SortLabel)),
            SortDirections.Descending => invoices.OrderByDescending(p => GetPropertyValue(p, request.SortLabel)),
            _ => invoices
        };

        var startIndex = request.Page * request.PageSize;
        invoices = invoices.Skip(startIndex).Take(request.PageSize);

        var response = await invoices
            .ProjectTo<GetInvoiceTableResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return response;
    }

    public async Task<GetInvoiceResponse?> GetInvoiceAsync(GetInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Id is 0)
            return null;

        var invoice = await _invoiceRepository.TableNoTracking
            .Include(x => x.Customer)
            .Include(x => x.InvoiceItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (invoice is null)
            return null;

        var invoiceDto = _mapper.Map<Invoice, GetInvoiceResponse>(invoice);

        return invoiceDto;
    }

    public async Task<ErrorOr<AddInvoiceResponse>> AddInvoiceAsync(AddInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        // 1. Add Customer
        var customer = _mapper.Map<Customer>(request);
        await _customerRepository.AddAsync(customer, cancellationToken);

        // 2. Add Invoice
        var invoice = _mapper.Map<Invoice>(request);
        await _invoiceRepository.AddAsync(invoice, cancellationToken);

        // 3. Add InvoiceItems
        var invoiceItems = _mapper.Map<List<InvoiceItem>>(request.InvoiceItems);
        await _invoiceItemRepository.AddRangeAsync(invoiceItems, cancellationToken);

        var response = _mapper.Map<AddInvoiceResponse>(invoice);

        return response;
    }

    public Task<ErrorOr<UpdateInvoiceResponse>> UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        // 1. Update Customer

        // 2. Update Invoice

        // 3. Update 
    }

    public async Task<ErrorOr<RemoveInvoiceResponse>> RemoveInvoiceAsync(RemoveInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.Id, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        await _invoiceRepository.DeleteAsync(invoice, cancellationToken);

        return new RemoveInvoiceResponse(invoice.Id);
    }

    public async Task<int> GetTotalInvoicesCount(CancellationToken cancellationToken)
        => await _invoiceRepository.TableNoTracking.CountAsync(cancellationToken);

    private static object? GetPropertyValue(object obj, string propertyName)
        => obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
}