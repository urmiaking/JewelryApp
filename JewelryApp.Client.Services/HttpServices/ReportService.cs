using ErrorOr;
using JewelryApp.Client.Services.Extensions;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Common;

namespace JewelryApp.Client.Services.HttpServices;

[ScopedService<IReportService>]
public class ReportService : IReportService
{
    private readonly HttpClient _authorizedClient;

    public ReportService(IHttpClientFactory httpClientFactory)
    {
        _authorizedClient = httpClientFactory.CreateClient("AuthorizedClient");
    }

    public async Task<ErrorOr<byte[]>> GetReportFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _authorizedClient.GetAsync($"{Urls.Reports}/{fileName}", cancellationToken);

            return await response.GenerateErrorOrResponseAsync<byte[]>(cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Failure(e.Message);
        }
    }
}