using ErrorOr;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Responses.Invoices;
using System.Net.Http.Json;
using JewelryApp.Client.Services.Extensions;
using JewelryApp.Shared.Attributes;

namespace JewelryApp.Client.Services.HttpServices;

[ScopedService<IInvoiceService>]
public class InvoiceService : IInvoiceService
{
    private readonly HttpClient _authorizedClient;

    public InvoiceService(IHttpClientFactory httpClientFactory)
    {
        _authorizedClient = httpClientFactory.CreateClient("AuthorizedClient");
    }

    public async Task<IEnumerable<GetInvoiceListResponse>?> GetInvoicesAsync(GetInvoiceListRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Invoices}?{nameof(request.PageSize)}={request.PageSize}&{nameof(request.Page)}={request.Page}&" +
                                                            $"{nameof(request.SortLabel)}={request.SortLabel}&{nameof(request.SortDirection)}={request.SortDirection}&" +
                                                            $"{nameof(request.SearchString)}={request.SearchString}", cancellationToken);

            return await response.GenerateResponseAsync<IEnumerable<GetInvoiceListResponse>?>(cancellationToken);
        }
        catch
        {
            return new List<GetInvoiceListResponse>();
        }
    }

    public async Task<ErrorOr<GetInvoiceResponse>> GetInvoiceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Invoices}/{id}", cancellationToken);

            return await response.GenerateErrorOrResponseAsync<GetInvoiceResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<AddInvoiceResponse>> AddInvoiceAsync(AddInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.PostAsJsonAsync(Urls.Invoices, request, cancellationToken);

            return await response.GenerateErrorOrResponseAsync<AddInvoiceResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<UpdateInvoiceResponse>> UpdateInvoiceAsync(UpdateInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.PutAsJsonAsync(Urls.Invoices, request, cancellationToken);

            return await response.GenerateErrorOrResponseAsync<UpdateInvoiceResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<RemoveInvoiceResponse>> RemoveInvoiceAsync(int id, bool deletePermanently = false, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.DeleteAsync($"{Urls.Invoices}/{id}/{deletePermanently}", cancellationToken);

            return await response.GenerateErrorOrResponseAsync<RemoveInvoiceResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<GetInvoicesCountResponse> GetTotalInvoicesCount(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Invoices}/Count", cancellationToken);

            return await response.GenerateResponseAsync<GetInvoicesCountResponse>(cancellationToken) ?? new GetInvoicesCountResponse(0);
        }
        catch
        {
            return new GetInvoicesCountResponse(0);
        }
    }

    public async Task<GetLastInvoiceNumberResponse> GetLastInvoiceNumber(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Invoices}/GetInvoiceNumber", cancellationToken);

            return await response.GenerateResponseAsync<GetLastInvoiceNumberResponse>(cancellationToken) ?? new GetLastInvoiceNumberResponse(1);
        }
        catch
        {
            return new GetLastInvoiceNumberResponse(1);
        }
    }
}