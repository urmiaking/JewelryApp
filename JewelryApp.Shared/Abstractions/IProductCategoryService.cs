﻿using ErrorOr;
using JewelryApp.Shared.Requests.ProductCategories;
using JewelryApp.Shared.Responses.ProductCategories;

namespace JewelryApp.Shared.Abstractions;

public interface IProductCategoryService
{
    Task<IEnumerable<GetProductCategoryResponse>> GetProductCategoriesAsync(CancellationToken cancellationToken = default);

    Task<ErrorOr<GetProductCategoryResponse>> GetProductCategoryByIdAsync(int id,
        CancellationToken cancellationToken = default);

    Task<ErrorOr<AddProductCategoryResponse>> AddProductCategoryAsync(AddProductCategoryRequest request, CancellationToken cancellationToken = default);

    Task<ErrorOr<UpdateProductCategoryResponse>> UpdateProductCategoryAsync(UpdateProductCategoryRequest request,
        CancellationToken cancellationToken = default);

    Task<ErrorOr<RemoveProductCategoryResponse>> RemoveProductCategoryAsync(int id, bool deletePermanently = false, CancellationToken cancellationToken = default);
}