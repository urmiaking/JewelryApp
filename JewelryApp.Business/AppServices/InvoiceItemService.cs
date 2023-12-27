﻿using AutoMapper;
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
    private readonly IProductRepository _productRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;

    public InvoiceItemService(IInvoiceItemRepository invoiceItemRepository, IMapper mapper, IInvoiceRepository invoiceRepository, IProductRepository productRepository)
    {
        _invoiceItemRepository = invoiceItemRepository;
        _mapper = mapper;
        _invoiceRepository = invoiceRepository;
        _productRepository = productRepository;
    }

    public async Task<ErrorOr<GetInvoiceItemResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var invoiceItem = await _invoiceItemRepository.GetByIdAsync(id, cancellationToken);

        if (invoiceItem is null)
            return Errors.InvoiceItem.NotFound;

        return _mapper.Map<GetInvoiceItemResponse>(invoiceItem);
    }

    public async Task<ErrorOr<IEnumerable<GetInvoiceItemResponse>>> GetInvoiceItemsByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;
        
        var invoiceItems = _invoiceItemRepository.GetInvoiceItemsByInvoiceId(invoiceId);

        //foreach ( var item in await invoiceItems.ToListAsync())
        //{
        //    await _invoiceItemRepository.LoadReferenceAsync(item, a => a.Product);
        //    await _invoiceItemRepository.LoadReferenceAsync(item, a => a.Invoice);
        //}

        var response = await invoiceItems.ProjectTo<GetInvoiceItemResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return response;
    }

    public async Task<ErrorOr<AddInvoiceItemResponse>> AddInvoiceItemAsync(AddInvoiceItemRequest request, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken);

        if (invoice is null)
            return Errors.Invoice.NotFound;

        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
            return Errors.Product.NotFound;

        var exists = await _invoiceItemRepository.CheckInvoiceItemExistsAsync(request.InvoiceId, request.ProductId, cancellationToken);

        if (exists)
            return Errors.InvoiceItem.Exists;

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

        return _mapper.Map<UpdateInvoiceItemResponse>(invoiceItem);
    }

    public async Task<ErrorOr<RemoveInvoiceItemResponse>> RemoveInvoiceItemAsync(int id, CancellationToken cancellationToken = default)
    {
        var invoiceItem = await _invoiceItemRepository.GetByIdAsync(id, cancellationToken);

        if (invoiceItem is null)
            return Errors.InvoiceItem.NotFound;

        if (invoiceItem.Deleted)
            return Errors.InvoiceItem.Deleted;
        
        await _invoiceItemRepository.DeleteAsync(invoiceItem, cancellationToken);

        return new RemoveInvoiceItemResponse(invoiceItem.Id);
    }
}