using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErrorOr;
using JewelryApp.Business.Interfaces;
using JewelryApp.Common.Constants;
using JewelryApp.Common.Enums;
using JewelryApp.Data.Interfaces.Repositories;
using JewelryApp.Data.Interfaces.Repositories.Base;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.InvoiceDtos;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Responses.Invoices;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.AppServices;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IMapper _mapper;

    public InvoiceService(IMapper mapper, IInvoiceRepository invoiceRepository, IRepository<Product> productRepository, IRepository<Customer> customerRepository)
    {
        _mapper = mapper;
        _invoiceRepository = invoiceRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
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
        var customer = new Customer
        {
            FullName = request.CustomerName,
            PhoneNumber = request.CustomerPhoneNumber
        };

        await _customerRepository.AddAsync(customer, cancellationToken);

        var invoice = _mapper.Map<Invoice>(request);

        await _invoiceRepository.AddAsync(invoice, cancellationToken);

        var response = _mapper.Map<AddInvoiceResponse>(invoice);

        return response;
    }

    public Task<ErrorOr<UpdateInvoiceResponse>> UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<RemoveInvoiceResponse>> RemoveInvoiceAsync(RemoveInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetTotalInvoicesCount(CancellationToken cancellationToken)
        => await _invoiceRepository.TableNoTracking.CountAsync(cancellationToken);

    private static object? GetPropertyValue(object obj, string propertyName)
        => obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
}