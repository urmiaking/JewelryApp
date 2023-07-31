using AutoMapper;
using AutoMapper.QueryableExtensions;
using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Common.Enums;
using JewelryApp.Data;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.AppServices;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<InvoiceProduct> _invoiceProductRepository;
    private readonly IBarcodeRepository _barcodeRepository;
    private readonly IMapper _mapper;

    public ProductService(IBarcodeRepository barcodeRepository, IMapper mapper, IRepository<Product> productRepository, IRepository<InvoiceProduct> invoiceProductRepository)
    {
        _barcodeRepository = barcodeRepository;
        _mapper = mapper;
        _productRepository = productRepository;
        _invoiceProductRepository = invoiceProductRepository;
    }

    public async Task<Product> SetProductAsync(SetProductDto productDto)
    {
        var productModel = _mapper.Map<SetProductDto, Product>(productDto);

        if (productModel.Id == 0)
        {
            productModel.BarcodeText = await _barcodeRepository.GetBarcodeAsync(productModel);
            productModel.AddedDateTime = DateTime.Now;
        }

        _productRepository.Update(productModel);

        return productModel;
    }

    public async Task<IEnumerable<ProductTableItemDto>> GetProductsAsync() => await _productRepository.TableNoTracking
            .OrderByDescending(a => a.AddedDateTime)
            .ProjectTo<ProductTableItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<DeleteResult> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(cancellationToken, id);

        if (product is null)
            return DeleteResult.IsNotAvailable;

        var canNotDelete = await _invoiceProductRepository.TableNoTracking
            .AnyAsync(a => a.ProductId == id);

        if (canNotDelete)
            return DeleteResult.CanNotDelete;
        
        await _productRepository.DeleteAsync(product, cancellationToken);
        return DeleteResult.Deleted;
    }
}