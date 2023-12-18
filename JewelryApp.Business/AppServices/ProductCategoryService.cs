using AutoMapper;
using AutoMapper.QueryableExtensions;
using ErrorOr;
using JewelryApp.Application.Interfaces;
using JewelryApp.Core.Attributes;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Errors;
using JewelryApp.Core.Interfaces.Repositories;
using JewelryApp.Shared.Requests.ProductCategories;
using JewelryApp.Shared.Responses.ProductCategories;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Application.AppServices;

[ScopedService<IProductCategoryService>]
public class ProductCategoryService : IProductCategoryService
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IMapper _mapper;

    public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IMapper mapper)
    {
        _productCategoryRepository = productCategoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetProductCategoryResponse>> GetProductCategoriesAsync(
        CancellationToken cancellationToken = default)
        => await _productCategoryRepository.Get()
            .ProjectTo<GetProductCategoryResponse>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    
    public async Task<ErrorOr<GetProductCategoryResponse>> GetProductCategoryAsync(GetProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var productCategory = await _productCategoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (productCategory is null)
            return Errors.ProductCategory.NotFound;

        return _mapper.Map<GetProductCategoryResponse>(productCategory);
    }

    public async Task<ErrorOr<AddProductCategoryResponse>> AddProductCategoryAsync(AddProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var productCategory = _mapper.Map<ProductCategory>(request);

        if (await _productCategoryRepository.CheckExistenceAsync(request.Name, cancellationToken))
            return Errors.ProductCategory.Exists;
        
        await _productCategoryRepository.AddAsync(productCategory, cancellationToken);

        return _mapper.Map<AddProductCategoryResponse>(productCategory);
    }

    public async Task<ErrorOr<UpdateProductCategoryResponse>> UpdateProductCategoryAsync(UpdateProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var productCategory = _mapper.Map<ProductCategory>(request);

        if (await _productCategoryRepository.CheckExistenceAsync(request.Name, cancellationToken))
            return Errors.ProductCategory.Exists;

        await _productCategoryRepository.UpdateAsync(productCategory, cancellationToken);

        return _mapper.Map<UpdateProductCategoryResponse>(productCategory);
    }

    public async Task<ErrorOr<RemoveProductCategoryResponse>> RemoveProductCategoryAsync(RemoveProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var productCategory = await _productCategoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (productCategory is null)
            return Errors.ProductCategory.NotFound;

        if (await _productCategoryRepository.CheckUsedAsync(productCategory.Id, cancellationToken))
            return Errors.ProductCategory.Used;

        await _productCategoryRepository.DeleteAsync(productCategory, cancellationToken);

        return new RemoveProductCategoryResponse(productCategory.Id);
    }
}