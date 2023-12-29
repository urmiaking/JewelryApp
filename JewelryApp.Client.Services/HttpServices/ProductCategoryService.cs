using ErrorOr;
using JewelryApp.Client.Services.Extensions;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Requests.ProductCategories;
using JewelryApp.Shared.Responses.ProductCategories;
using System.Net.Http.Json;
using JewelryApp.Shared.Attributes;

namespace JewelryApp.Client.Services.HttpServices;

[ScopedService<IProductCategoryService>]
public class ProductCategoryService : IProductCategoryService
{
    private readonly HttpClient _authorizedClient;

    public ProductCategoryService(IHttpClientFactory httpClientFactory)
    {
        _authorizedClient = httpClientFactory.CreateClient("AuthorizedClient");
    }

    public async Task<IEnumerable<GetProductCategoryResponse>> GetProductCategoriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.ProductCategories}", cancellationToken);

            return await response.GenerateResponseAsync<IEnumerable<GetProductCategoryResponse>>(cancellationToken) ??
                   Array.Empty<GetProductCategoryResponse>();
        }
        catch
        {
            return Array.Empty<GetProductCategoryResponse>();
        }
    }

    public async Task<ErrorOr<GetProductCategoryResponse>> GetProductCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.ProductCategories}/{id}", cancellationToken);

            return await response.GenerateErrorOrResponseAsync<GetProductCategoryResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<AddProductCategoryResponse>> AddProductCategoryAsync(AddProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.PostAsJsonAsync(Urls.ProductCategories, request, cancellationToken);

            return await response.GenerateErrorOrResponseAsync<AddProductCategoryResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<UpdateProductCategoryResponse>> UpdateProductCategoryAsync(UpdateProductCategoryRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.PutAsJsonAsync(Urls.ProductCategories, request, cancellationToken);

            return await response.GenerateErrorOrResponseAsync<UpdateProductCategoryResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<RemoveProductCategoryResponse>> RemoveProductCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.DeleteAsync($"{Urls.ProductCategories}/{id}", cancellationToken);

            return await response.GenerateErrorOrResponseAsync<RemoveProductCategoryResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }
}