﻿using ErrorOr;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Products;

namespace JewelryApp.Application.Interfaces;

public interface IProductService
{
    Task<ErrorOr<AddProductResponse>> AddProductAsync(AddProductRequest request, CancellationToken token = default);
    Task<ErrorOr<UpdateProductResponse>> UpdateProductAsync(UpdateProductRequest request, CancellationToken token = default);
    Task<ErrorOr<RemoveProductResponse>> RemoveProductAsync(int id, CancellationToken token = default);
    Task<IEnumerable<GetProductResponse>?> GetProductsAsync(GetProductsRequest request, CancellationToken token = default);
    Task<GetProductsCountResponse> GetTotalProductsCount(CancellationToken cancellationToken = default);
    Task<GetProductResponse?> GetProductByBarcodeAsync(string barcode, CancellationToken token = default);
    Task<GetProductResponse?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
}