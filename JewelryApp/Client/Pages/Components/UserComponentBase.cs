using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using AutoMapper;

namespace JewelryApp.Client.Pages.Components;

public abstract class UserComponentBase : ComponentBase
{
    private CancellationTokenSource? _cancellationTokenSource;

    protected CancellationTokenSource CancellationTokenSource
    {
        get
        {
            _cancellationTokenSource ??= new CancellationTokenSource();

            return _cancellationTokenSource;
        }
    }

    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
    [Inject] protected ISnackbar SnackBar { get; set; } = default!;
    [Inject] protected IMapper Mapper { get; set; } = default!;

    protected void CancelToken()
    {
        CancellationTokenSource.Cancel();

        DestroyCancellationToken();
    }

    protected void DestroyCancellationToken()
    {
        CancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }
}