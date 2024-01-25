using System.Net.Http.Json;
using ErrorOr;
using JewelryApp.Client.Services.Extensions;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Requests.OldGolds;
using JewelryApp.Shared.Responses.OldGolds;

namespace JewelryApp.Client.Services.HttpServices;

[ScopedService<IOldGoldService>]
public class OldGoldService : IOldGoldService
{
    private readonly HttpClient _authorizedClient;

    public OldGoldService(IHttpClientFactory httpClientFactory)
    {
        _authorizedClient = httpClientFactory.CreateClient("AuthorizedClient");
    }

    public async Task<ErrorOr<AddOldGoldResponse>> AddOldGoldAsync(AddOldGoldRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.PostAsJsonAsync(Urls.OldGolds, request, cancellationToken);

            return await response.GenerateErrorOrResponseAsync<AddOldGoldResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<List<GetOldGoldResponse>>> GetOldGoldsByInvoiceIdAsync(int invoiceId, CancellationToken token = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.OldGolds}/{invoiceId}", token);

            return await response.GenerateErrorOrResponseAsync<List<GetOldGoldResponse>>(token);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }

    public async Task<ErrorOr<RemoveOldGoldResponse>> RemoveOldGoldAsync(int id, bool deletePermanently = false, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.DeleteAsync($"{Urls.OldGolds}/{id}/{deletePermanently}", cancellationToken);

            return await response.GenerateErrorOrResponseAsync<RemoveOldGoldResponse>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(description: e.Message);
        }
    }
}