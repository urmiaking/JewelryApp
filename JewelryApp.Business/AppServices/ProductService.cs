using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErrorOr;
using JewelryApp.Core.Constants;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Errors;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Products;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Application.AppServices;

[ScopedService<IProductService>]
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IInvoiceItemRepository _invoiceItemRepository;
    private readonly IMapper _mapper;

    public ProductService(IMapper mapper, IProductRepository productRepository, IInvoiceItemRepository invoiceItemRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
        _invoiceItemRepository = invoiceItemRepository;
    }

    public async Task<ErrorOr<AddProductResponse>> AddProductAsync(AddProductRequest request, CancellationToken token = default)
    {
        var product = _mapper.Map<Product>(request);

        var barcodeExists = await _productRepository.CheckBarcodeExistsAsync(product.Barcode, token);

        if (barcodeExists)
            return Errors.Product.BarcodeExists;

        await _productRepository.AddAsync(product, token);
        await _productRepository.LoadReferenceAsync(product, x => x.ProductCategory, token);

        var response = _mapper.Map<AddProductResponse>(product);

        return response;
    }

    public async Task<ErrorOr<UpdateProductResponse>> UpdateProductAsync(UpdateProductRequest request, CancellationToken token = default)
    {
        var product = _mapper.Map<Product>(request);

        await _productRepository.UpdateAsync(product, token);

        var response = _mapper.Map<UpdateProductResponse>(product);

        return response;
    }

    public async Task<ErrorOr<RemoveProductResponse>> RemoveProductAsync(int id, bool deletePermanently = false, CancellationToken token = default)
    {
        var product = await _productRepository.GetByIdAsync(id, token);

        if (product is null)
            return Errors.Product.NotFound;

        if (product.Deleted && !deletePermanently)
            return Errors.Product.Deleted;

        var isSold = await _invoiceItemRepository.CheckProductIsSoldAsync(product.Id, token);

        if (isSold)
            return Errors.Product.Sold;

        await _productRepository.DeleteAsync(product, token, deletePermanently: deletePermanently);

        return new RemoveProductResponse(product.Id);
    }

    public async Task<IEnumerable<GetProductResponse>?> GetProductsAsync(GetProductsRequest request, CancellationToken token = default)
    {
        var products = _productRepository.GetProductsInStock();

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            products = products.Where(a => a.Barcode.Contains(request.SearchString) ||
                                           a.Name.Contains(request.SearchString));
        }

        if (!string.IsNullOrEmpty(request.SortLabel))
        {
            products = request.SortDirection switch
            {
                SortDirections.Ascending => products.OrderBy(p => GetPropertyValue(p, request.SortLabel)),
                SortDirections.Descending => products.OrderByDescending(p => GetPropertyValue(p, request.SortLabel)),
                _ => products
            };
        }

        var startIndex = request.Page * request.PageSize;
        products = products.Skip(startIndex).Take(request.PageSize);

        return await products.ProjectTo<GetProductResponse>(_mapper.ConfigurationProvider).ToListAsync(token);
    }

    public async Task<GetProductsCountResponse> GetTotalProductsCount(CancellationToken cancellationToken = default)
    {
        var count = await _productRepository.GetProductsInStock().CountAsync(cancellationToken);

        return new GetProductsCountResponse(count);
    }

    public async Task<ErrorOr<GetProductResponse>> GetProductByBarcodeAsync(string barcode, CancellationToken token = default)
    {
        var product = await _productRepository.GetByBarcodeAsync(barcode, token);

        if (product is null)
            return Errors.Product.NotFound;

        var productSold = await _invoiceItemRepository.CheckProductIsSoldAsync(product.Id, token);

        if (productSold)
            return Errors.Product.Sold;

        await _productRepository.LoadReferenceAsync(product, x => x.ProductCategory, token);
        var response = _mapper.Map<Product, GetProductResponse>(product);

        return response;
    }

    public async Task<IEnumerable<GetProductResponse>?> GetProductsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var products = _productRepository.Get().OrderByDescending(x => x.CreatedAt).Where(x => x.Name.Contains(name));

        return (await products.ProjectTo<GetProductResponse>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken)).DistinctBy(x => x.Name);
    }

    public async Task<ErrorOr<GetProductResponse>> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            return Errors.Product.NotFound;

        await _productRepository.LoadReferenceAsync(product, x => x.ProductCategory, cancellationToken);
        var response = _mapper.Map<Product, GetProductResponse>(product);

        return response;
    }

    private static object? GetPropertyValue(object obj, string propertyName)
        => obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
}