using AutoMapper;
using AutoMapper.QueryableExtensions;
using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Common.Enums;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.Invoice;
using JewelryApp.Models.Dtos.Product;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.AppServices;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<InvoiceItem> _invoiceProductRepository;
    private readonly IBarcodeRepository _barcodeRepository;
    private readonly IMapper _mapper;

    public ProductService(IBarcodeRepository barcodeRepository, IMapper mapper, IRepository<Product> productRepository, IRepository<InvoiceItem> invoiceProductRepository)
    {
        _barcodeRepository = barcodeRepository;
        _mapper = mapper;
        _productRepository = productRepository;
        _invoiceProductRepository = invoiceProductRepository;
    }

    public async Task<Product> SetProductAsync(ProductDto productDto)
    {
        var productModel = _mapper.Map<ProductDto, Product>(productDto);

        if (productModel.Id == 0)
        {
            productModel.Barcode = await _barcodeRepository.GetBarcodeAsync(productModel);
            productModel.CreatedAt = DateTime.Now;
        }

        _productRepository.Update(productModel);

        return productModel;
    }

    public async Task<IEnumerable<ProductTableItemDto>> GetProductsAsync(int page, int pageSize, string sortDirection, 
        string sortLabel, string searchString, CancellationToken cancellationToken)
    {
        var products =  await _productRepository.TableNoTracking
            .ProjectTo<ProductTableItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(a => a.BarcodeText.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                           a.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        products = sortDirection switch
        {
            "Ascending" => products.OrderBy(p => GetPropertyValue(p, sortLabel)).ToList(),
            "Descending" => products.OrderByDescending(p => GetPropertyValue(p, sortLabel)).ToList(),
            _ => products
        };

        var startIndex = page * pageSize;
        products = products.Skip(startIndex).Take(pageSize).ToList();

        return products;
    }
    
    private static object GetPropertyValue(object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
    }

    public async Task<DeleteResult> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        if (id is 0)
            return DeleteResult.IsNotAvailable;

        var product = await _productRepository.GetByIdAsync(cancellationToken, id);

        if (product is null)
            return DeleteResult.IsNotAvailable;

        var canNotDelete = await _invoiceProductRepository.TableNoTracking
            .AnyAsync(a => a.ProductId == id, cancellationToken);

        if (canNotDelete)
            return DeleteResult.CanNotDelete;

        await _productRepository.DeleteAsync(product, cancellationToken);
        return DeleteResult.Deleted;
    }

    public async Task<int> GetTotalProductsCount(CancellationToken cancellationToken)
        => await _productRepository.TableNoTracking.CountAsync(cancellationToken);

    public async Task<InvoiceItemDto> GetProductByBarcodeAsync(string barcodeText)
    {
        var product =
            await _productRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Barcode.Equals(barcodeText));

        if (product is null)
            return null;

        var productDto = _mapper.Map<Product, InvoiceItemDto>(product);

        return productDto;
    }
}