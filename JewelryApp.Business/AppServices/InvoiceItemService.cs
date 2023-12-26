using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErrorOr;
using JewelryApp.Application.Interfaces;
using JewelryApp.Core.Attributes;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Errors;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Shared.Requests.InvoiceItems;
using JewelryApp.Shared.Responses.InvoiceItems;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Application.AppServices;

[ScopedService<IInvoiceItemService>]
public class InvoiceItemService : IInvoiceItemService
{
    private readonly IInvoiceItemRepository _invoiceItemRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;

    public InvoiceItemService(IInvoiceItemRepository invoiceItemRepository, IMapper mapper, IInvoiceRepository invoiceRepository)
    {
        _invoiceItemRepository = invoiceItemRepository;
        _mapper = mapper;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<IEnumerable<GetInvoiceItemResponse>> GetInvoiceItemsAsync(GetInvoiceItemsRequest request, CancellationToken cancellationToken = default)
    {
        var invoiceItems = _invoiceItemRepository.GetInvoiceItemsByInvoiceId(request.InvoiceId);

        var response = await invoiceItems.ProjectTo<GetInvoiceItemResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return response;
    }

    public async Task<ErrorOr<AddInvoiceItemResponse>> AddInvoiceItemAsync(AddInvoiceItemRequest request, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        var invoiceItem = _mapper.Map<InvoiceItem>(request);

        await _invoiceItemRepository.AddAsync(invoiceItem, cancellationToken);

        return _mapper.Map<AddInvoiceItemResponse>(invoiceItem);
    }

    public async Task<ErrorOr<UpdateInvoiceItemResponse>> UpdateInvoiceItemAsync(UpdateInvoiceItemRequest request, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        var invoiceItem = await _invoiceItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (invoiceItem is null)
            return Errors.InvoiceItem.NotFound;

        invoiceItem = _mapper.Map<InvoiceItem>(request);

        await _invoiceItemRepository.UpdateAsync(invoiceItem, cancellationToken);

        return new UpdateInvoiceItemResponse(invoiceItem.Id, invoiceItem.InvoiceId, invoiceItem.ProductId);
    }

    public async Task<ErrorOr<RemoveInvoiceItemResponse>> RemoveInvoiceItemAsync(RemoveInvoiceItemRequest request, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        var invoiceItem = await _invoiceItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (invoiceItem is null)
            return Errors.InvoiceItem.NotFound;

        await _invoiceItemRepository.DeleteAsync(invoiceItem, cancellationToken);

        return new RemoveInvoiceItemResponse(invoiceItem.Id, invoiceItem.InvoiceId, invoiceItem.ProductId);
    }
}