using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components;

public abstract class UserComponentBase : ComponentBase
{
    private HttpClient? _authorizedHttpClient;
    private HttpClient? _unauthorizedHttpClient;

    [Inject] private IHttpClientFactory ClientFactory { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
    [Inject] protected ISnackbar SnackBar { get; set; } = default!;

    protected HttpClient AuthorizedHttpClient
    {
        get
        {
            if (_authorizedHttpClient == null)
                _authorizedHttpClient = ClientFactory.CreateClient("AuthorizedClient");

            return _authorizedHttpClient;
        }
    }
    protected HttpClient UnauthorizedHttpClient
    {
        get
        {
            if (_unauthorizedHttpClient == null)
                _unauthorizedHttpClient = ClientFactory.CreateClient("UnauthorizedClient");

            return _unauthorizedHttpClient;
        }
    }

    protected override void OnInitialized()
    {
        if (ClientFactory is null)
            throw new InvalidOperationException("No ClientFactory is registered.");

        base.OnInitialized();
    }
}