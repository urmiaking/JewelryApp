using AutoMapper;
using AutoMapper.QueryableExtensions;
using JewelryApp.Business.Interfaces;
using JewelryApp.Common.Enums;
using JewelryApp.Data.Interfaces.Repositories.Base;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.InvoiceDtos;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.AppServices;

public class InvoiceService : IInvoiceService
{
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly IRepository<InvoiceItem> _invoiceProductRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IMapper _mapper;

    public InvoiceService(IMapper mapper, IRepository<Invoice> invoiceRepository, IRepository<InvoiceItem> invoiceProductRepository, IRepository<Product> productRepository, IRepository<Customer> customerRepository)
    {
        _mapper = mapper;
        _invoiceRepository = invoiceRepository;
        _invoiceProductRepository = invoiceProductRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<InvoiceTableItemDto>> GetInvoicesAsync(int page, int pageSize, string sortDirection, string sortLabel, string searchString, CancellationToken cancellationToken)
    {
        var invoices = await _invoiceRepository.TableNoTracking
            .Include(a => a.InvoiceItems)
            .ProjectTo<InvoiceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var invoiceItems = _mapper.Map<List<InvoiceDto>, List<InvoiceTableItemDto>>(invoices);

        if (!string.IsNullOrEmpty(searchString))
        {
            invoiceItems = invoiceItems.Where(a => a.CustomerName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                                   a.CustomerPhone.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                                   a.BuyDate.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        invoiceItems = sortDirection switch
        {
            "Ascending" => invoiceItems.OrderBy(p => GetPropertyValue(p, sortLabel)).ToList(),
            "Descending" => invoiceItems.OrderByDescending(p => GetPropertyValue(p, sortLabel)).ToList(),
            _ => invoiceItems
        };

        var startIndex = page * pageSize;
        invoiceItems = invoiceItems.Skip(startIndex).Take(pageSize).ToList();

        return invoiceItems;
    }

    private static object? GetPropertyValue(object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
    }

    public async Task<InvoiceDto?> GetInvoiceAsync(int id, CancellationToken cancellationToken)
    {
        if (id is 0)
            return null;

        var invoice = await _invoiceRepository.TableNoTracking
            .Include(x => x.Customer)
            .Include(x => x.InvoiceItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (invoice is null)
            return null;

        var invoiceDto = _mapper.Map<Invoice, InvoiceDto>(invoice);

        return invoiceDto;
    }

    public async Task<bool> SetInvoiceAsync(InvoiceDto invoiceDto, CancellationToken cancellationToken)
    {
        try
        {
            var invoice = _mapper.Map<InvoiceDto, Invoice>(invoiceDto);

            // Adding Customer
            var availableCustomer =
                await _customerRepository.Table.FirstOrDefaultAsync(
                    x => x.FullName == invoice.Customer.FullName && x.PhoneNumber == invoice.Customer.PhoneNumber,
                    cancellationToken);

            if (availableCustomer is not null)
            {
                invoice.CustomerId = availableCustomer.Id;
            }
            else
            {
                await _customerRepository.AddAsync(invoice.Customer, cancellationToken);
            }

            // Adding Invoice
            if (invoice.Id is 0)
            {
                await _invoiceRepository.AddAsync(invoice, cancellationToken);
            }
            else // Updating Invoice
            {
                await _invoiceRepository.UpdateAsync(invoice, cancellationToken, false);

                var invoiceItems = await _invoiceProductRepository.Table
                    .Where(a => a.InvoiceId == invoiceDto.Id).ToListAsync(cancellationToken);

                await _invoiceProductRepository.DeleteRangeAsync(invoiceItems, cancellationToken);
            }

            foreach (var productDto in invoiceDto.Products)
            {
                var product = _mapper.Map<InvoiceItemDto, Product>(productDto);

                if (product.Id == 0)
                {
                    await _productRepository.AddAsync(product, cancellationToken);
                }

                var invoiceProduct = new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    ProductId = product.Id,
                    Quantity = productDto.Quantity,
                    Profit = productDto.Profit / 100.0,
                    TaxOffset = productDto.TaxOffset / 100.0
                };

                await _invoiceProductRepository.AddAsync(invoiceProduct, cancellationToken);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<DeleteResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id is 0)
            return DeleteResult.IsNotAvailable;

        var invoice = await _invoiceRepository.GetByIdAsync(cancellationToken, id);

        if (invoice is null)
            return DeleteResult.IsNotAvailable;

        try
        {
            await _invoiceRepository.DeleteAsync(invoice, cancellationToken);

            return DeleteResult.Deleted;
        }
        catch
        {
            return DeleteResult.CanNotDelete;
        }
    }

    public async Task<int> GetTotalInvoicesCount(CancellationToken cancellationToken)
        => await _invoiceRepository.TableNoTracking.CountAsync(cancellationToken);

    public async Task<bool> UpdateInvoiceHeaderAsync(InvoiceHeaderDto invoiceHeaderDto, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.TableNoTracking
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == invoiceHeaderDto.InvoiceId, cancellationToken);

        if (invoice is null) 
            return false;

        var customer = invoice.Customer;

        customer.FullName = invoiceHeaderDto.CustomerName;
        customer.PhoneNumber = invoiceHeaderDto.CustomerPhone;

        await _customerRepository.UpdateAsync(customer, cancellationToken);

        return true;
    }
}