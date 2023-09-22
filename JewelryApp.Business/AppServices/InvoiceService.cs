using AutoMapper;
using AutoMapper.QueryableExtensions;
using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Common.Enums;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.AppServices;

public class InvoiceService : IInvoiceService
{
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly IRepository<InvoiceProduct> _invoiceProductRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IBarcodeRepository _barcodeRepository;
    private readonly IMapper _mapper;

    public InvoiceService(IMapper mapper, IBarcodeRepository barcodeRepository, IRepository<Invoice> invoiceRepository, IRepository<InvoiceProduct> invoiceProductRepository, IRepository<Product> productRepository)
    {
        _mapper = mapper;
        _barcodeRepository = barcodeRepository;
        _invoiceRepository = invoiceRepository;
        _invoiceProductRepository = invoiceProductRepository;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<InvoiceTableItemDto>> GetInvoicesAsync(int page, int pageSize, string sortDirection, string sortLabel, string searchString, CancellationToken cancellationToken)
    {
        var invoices = await _invoiceRepository.TableNoTracking
            .Include(a => a.InvoiceProducts)
            .ProjectTo<InvoiceTableItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (!string.IsNullOrEmpty(searchString))
        {
            invoices = invoices.Where(a => a.BuyerName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                           a.BuyerPhone.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                           a.BuyDate.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        invoices = sortDirection switch
        {
            "Ascending" => invoices.OrderBy(p => GetPropertyValue(p, sortLabel)).ToList(),
            "Descending" => invoices.OrderByDescending(p => GetPropertyValue(p, sortLabel)).ToList(),
            _ => invoices
        };

        var startIndex = page * pageSize;
        invoices = invoices.Skip(startIndex).Take(pageSize).ToList();

        return invoices;
    }

    private static object GetPropertyValue(object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
    }

    public async Task<InvoiceDto> GetInvoiceAsync(int id, CancellationToken cancellationToken)
    {
        if (id is 0)
            return null;

        var invoice = await _invoiceRepository.TableNoTracking
            .Include(x => x.InvoiceProducts)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (invoice is null)
            return null;

        var invoiceDto = _mapper.Map<Invoice, InvoiceDto>(invoice);

        foreach (var invoiceProduct in invoice.InvoiceProducts)
        {
            var product = invoiceProduct.Product;

            invoiceDto.Products.Add(new ProductDto
            {
                Caret = product.Caret,
                Count = invoiceProduct.Count,
                FinalPrice = invoiceProduct.FinalPrice,
                GramPrice = invoiceProduct.GramPrice,
                Id = product.Id,
                ProductType = product.ProductType,
                Name = product.Name,
                Profit = Math.Round(invoiceProduct.Profit * 100, 0),
                TaxOffset = Math.Round(invoiceProduct.TaxOffset * 100, 0),
                Wage = product.Wage,
                Weight = product.Weight
            });
        }

        return invoiceDto;
    }

    public async Task<bool> SetInvoiceAsync(InvoiceDto invoiceDto, CancellationToken cancellationToken)
    {
        try
        {
            var invoice = _mapper.Map<InvoiceDto, Invoice>(invoiceDto);

            if (invoice.Id is 0)
            {
                await _invoiceRepository.AddAsync(invoice, cancellationToken);
            }
            else
            {
                await _invoiceRepository.UpdateAsync(invoice, cancellationToken, false);

                var invoiceProducts = await _invoiceProductRepository.Table
                    .Where(a => a.InvoiceId == invoiceDto.Id).ToListAsync(cancellationToken);

                await _invoiceProductRepository.DeleteRangeAsync(invoiceProducts, cancellationToken);
            }

            foreach (var productDto in invoiceDto.Products)
            {
                var product = _mapper.Map<ProductDto, Product>(productDto);
                productDto.GramPrice = invoiceDto.GramPrice;

                if (product.Id == 0)
                {
                    product.BarcodeText = await _barcodeRepository.GetBarcodeAsync(product);
                    product.AddedDateTime = DateTime.Now;
                    await _productRepository.AddAsync(product, cancellationToken);
                }

                var invoiceProduct = new InvoiceProduct
                {
                    InvoiceId = invoice.Id,
                    ProductId = product.Id,
                    Count = productDto.Count,
                    Profit = productDto.Profit / 100.0,
                    GramPrice = invoiceDto.GramPrice,
                    TaxOffset = productDto.TaxOffset / 100.0,
                    FinalPrice = productDto.FinalPrice,
                    Tax = productDto.Tax
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

    public async Task<bool> UpdateInvoiceHeaderAsync(InvoiceHeader invoiceHeader, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(cancellationToken, invoiceHeader.InvoiceId);

        if (invoice is null) 
            return false;

        var nameArray = invoiceHeader.BuyerName.Split(" ");

        if (nameArray.Length > 1)
        {
            invoice.BuyerFirstName = nameArray[0];
            invoice.BuyerLastName = string.Join(" ", nameArray.Skip(1));
        }
        else
        {
            invoice.BuyerFirstName = string.Empty;
            invoice.BuyerLastName = invoiceHeader.BuyerName;
        }

        invoice.BuyerPhoneNumber = invoiceHeader.BuyerPhone;

        await _invoiceRepository.UpdateAsync(invoice, cancellationToken);

        return true;
    }
}