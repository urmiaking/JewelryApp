using System.Net.Http.Json;
using ErrorOr;
using JewelryApp.Client.Services.Extensions;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Products;

namespace JewelryApp.Client.Services.HttpServices;

[ScopedService<IProductService>]
public class ProductService : IProductService
{
    private readonly HttpClient _authorizedClient;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        _authorizedClient = httpClientFactory.CreateClient("AuthorizedClient");
    }

    public async Task<ErrorOr<AddProductResponse>> AddProductAsync(AddProductRequest request, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.PostAsJsonAsync(Urls.Products, request, token);

            return await response.GenerateErrorOrResponseAsync<AddProductResponse>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<UpdateProductResponse>> UpdateProductAsync(UpdateProductRequest request, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.PutAsJsonAsync(Urls.Products, request, token);

            return await response.GenerateErrorOrResponseAsync<UpdateProductResponse>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<RemoveProductResponse>> RemoveProductAsync(int id, bool deletePermanently = false, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.DeleteAsync($"{Urls.Products}/{id}/{deletePermanently}", token);

            return await response.GenerateErrorOrResponseAsync<RemoveProductResponse>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<IEnumerable<GetProductResponse>?> GetProductsAsync(GetProductsRequest request, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Products}?{nameof(request.PageSize)}={request.PageSize}&{nameof(request.Page)}={request.Page}&" +
                                                            $"{nameof(request.SortLabel)}={request.SortLabel}&{nameof(request.SortDirection)}={request.SortDirection}&" +
                                                            $"{nameof(request.SearchString)}={request.SearchString}", token);

            return await response.GenerateResponseAsync<IEnumerable<GetProductResponse>?>(token);
        }
        catch
        {
            return new List<GetProductResponse>();
        }
    }

    public async Task<GetProductsCountResponse> GetTotalProductsCount(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Products}/Count", cancellationToken);

            return await response.GenerateResponseAsync<GetProductsCountResponse>(cancellationToken) ?? new GetProductsCountResponse(0);
        }
        catch
        {
            return new GetProductsCountResponse(0);
        }
    }

    public async Task<ErrorOr<GetProductResponse>> GetProductByBarcodeAsync(string barcode, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Products}/barcode/{barcode}", token);

            return await response.GenerateErrorOrResponseAsync<GetProductResponse>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<GetProductResponse>> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Products}/{id}", cancellationToken);

            return await response.GenerateErrorOrResponseAsync<GetProductResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<IEnumerable<GetProductResponse>?> GetProductsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Products}/name/{name}", cancellationToken);

            return await response.GenerateResponseAsync<List<GetProductResponse>>(cancellationToken);
        }
        catch (Exception e)
        {
            return new List<GetProductResponse>();
        }
    }
}