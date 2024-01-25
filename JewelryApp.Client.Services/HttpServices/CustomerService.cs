using System.Net.Http.Json;
using ErrorOr;
using JewelryApp.Client.Services.Extensions;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Requests.Customer;
using JewelryApp.Shared.Responses.Customer;

namespace JewelryApp.Client.Services.HttpServices;

[ScopedService<ICustomerService>]
public class CustomerService : ICustomerService
{
    private readonly HttpClient _authorizedClient;

    public CustomerService(IHttpClientFactory httpClientFactory)
    {
        _authorizedClient = httpClientFactory.CreateClient("AuthorizedClient");
    }

    public async Task<ErrorOr<AddCustomerResponse>> AddCustomerAsync(AddCustomerRequest request, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.PostAsJsonAsync(Urls.Customers, request, token);

            return await response.GenerateErrorOrResponseAsync<AddCustomerResponse>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.PutAsJsonAsync(Urls.Customers, request, token);

            return await response.GenerateErrorOrResponseAsync<UpdateCustomerResponse>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<RemoveCustomerResponse>> RemoveCustomerAsync(int id, bool deletePermanently = false, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.DeleteAsync($"{Urls.Customers}/{id}/{deletePermanently}", token);

            return await response.GenerateErrorOrResponseAsync<RemoveCustomerResponse>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<GetCustomerResponse>> GetCustomerByInvoiceIdAsync(int id, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Customers}/invoice/{id}", token);

            return await response.GenerateErrorOrResponseAsync<GetCustomerResponse>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<GetCustomerResponse>> GetCustomerByPhoneNumberAsync(string phoneNumber, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Customers}/phoneNumber/{phoneNumber}", token);

            return await response.GenerateErrorOrResponseAsync<GetCustomerResponse>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<GetCustomerResponse>> GetCustomerByNationalCodeAsync(string nationalCode,
        CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Customers}/nationalCode/{nationalCode}", token);

            return await response.GenerateErrorOrResponseAsync<GetCustomerResponse>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<GetCustomerResponse>> GetCustomerByIdAsync(int id, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Customers}/{id}", token);

            return await response.GenerateErrorOrResponseAsync<GetCustomerResponse>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }
}