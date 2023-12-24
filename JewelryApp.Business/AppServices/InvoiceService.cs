using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErrorOr;
using JewelryApp.Application.Interfaces;
using JewelryApp.Core.Attributes;
using JewelryApp.Core.Constants;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Errors;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Responses.Invoices;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Application.AppServices;

[ScopedService<IInvoiceService>]
public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;

    public InvoiceService(IMapper mapper, IInvoiceRepository invoiceRepository)
    {
        _mapper = mapper;
        _invoiceRepository = invoiceRepository;
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

        await _invoiceRepository.AddAsync(invoice, cancellationToken);

        var response = _mapper.Map<AddInvoiceResponse>(invoice);

        return response;
    }

    public async Task<ErrorOr<UpdateInvoiceResponse>> UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.Id, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        invoice = _mapper.Map<Invoice>(request);

        await _invoiceRepository.UpdateAsync(invoice, cancellationToken);

        return new UpdateInvoiceResponse(invoice.Id);
    }

    public async Task<ErrorOr<RemoveInvoiceResponse>> RemoveInvoiceAsync(int id, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        await _invoiceRepository.DeleteAsync(invoice, cancellationToken);

        return new RemoveInvoiceResponse(invoice.Id);
    }

    public async Task<int> GetTotalInvoicesCount(CancellationToken cancellationToken)
        => await _invoiceRepository.Get().CountAsync(cancellationToken);

    private static object? GetPropertyValue(object obj, string propertyName)
        => obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
}