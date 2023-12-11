using ErrorOr;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Products;

namespace JewelryApp.Application.Interfaces;

public interface IProductService
{
    Task<ErrorOr<AddProductResponse>> AddProductAsync(AddProductRequest request, CancellationToken token = default);
    Task<ErrorOr<UpdateProductResponse>> UpdateProductAsync(UpdateProductRequest request, CancellationToken token = default);
    Task<ErrorOr<RemoveProductResponse>> RemoveProductAsync(RemoveProductRequest request, CancellationToken token = default);
    Task<IEnumerable<GetProductResponse>?> GetProductsAsync(GetProductsRequest request, CancellationToken token = default);
    Task<int> GetTotalProductsCount(CancellationToken cancellationToken = default);
    Task<GetProductResponse?> GetProductByBarcodeAsync(string barcode, CancellationToken token = default);
}