using ErrorOr;
using JewelryApp.Client.Services.Extensions;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Requests.InvoiceItems;
using JewelryApp.Shared.Responses.InvoiceItems;
using System.Net.Http.Json;
using JewelryApp.Shared.Attributes;

namespace JewelryApp.Client.Services.HttpServices;

[ScopedService<IInvoiceItemService>]
public class InvoiceItemService : IInvoiceItemService
{
    private readonly HttpClient _authorizedClient;

    public InvoiceItemService(IHttpClientFactory httpClientFactory)
    {
        _authorizedClient = httpClientFactory.CreateClient("AuthorizedClient");
    }

    public async Task<ErrorOr<GetInvoiceItemResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.InvoiceItems}/{id}", cancellationToken);

            return await response.GenerateErrorOrResponseAsync<GetInvoiceItemResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<IEnumerable<GetInvoiceItemResponse>>> GetInvoiceItemsByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.InvoiceItems}/invoice/{invoiceId}", cancellationToken);

            return await response.GenerateErrorOrResponseAsync<IEnumerable<GetInvoiceItemResponse>>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<AddInvoiceItemResponse>> AddInvoiceItemAsync(AddInvoiceItemRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.PostAsJsonAsync(Urls.InvoiceItems, request, cancellationToken);

            return await response.GenerateErrorOrResponseAsync<AddInvoiceItemResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<UpdateInvoiceItemResponse>> UpdateInvoiceItemAsync(UpdateInvoiceItemRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.PutAsJsonAsync(Urls.InvoiceItems, request, cancellationToken);

            return await response.GenerateErrorOrResponseAsync<UpdateInvoiceItemResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<RemoveInvoiceItemResponse>> RemoveInvoiceItemAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.DeleteAsync($"{Urls.Invoices}/{id}", cancellationToken);

            return await response.GenerateErrorOrResponseAsync<RemoveInvoiceItemResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }
}