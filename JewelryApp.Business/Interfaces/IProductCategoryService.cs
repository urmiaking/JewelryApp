using ErrorOr;
using JewelryApp.Shared.Requests.ProductCategories;
using JewelryApp.Shared.Responses.ProductCategories;

namespace JewelryApp.Application.Interfaces;

public interface IProductCategoryService
{
    Task<IEnumerable<GetProductCategoryResponse>> GetProductCategoriesAsync(CancellationToken cancellationToken = default);

    Task<ErrorOr<GetProductCategoryResponse>> GetProductCategoryAsync(GetProductCategoryRequest request,
        CancellationToken cancellationToken = default);

    Task<ErrorOr<AddProductCategoryResponse>> AddProductCategoryAsync(AddProductCategoryRequest request, CancellationToken cancellationToken = default);

    Task<ErrorOr<UpdateProductCategoryResponse>> UpdateProductCategoryAsync(UpdateProductCategoryRequest request,
        CancellationToken cancellationToken = default);

    Task<ErrorOr<RemoveProductCategoryResponse>> RemoveProductCategoryAsync(RemoveProductCategoryRequest request, CancellationToken cancellationToken = default);
}