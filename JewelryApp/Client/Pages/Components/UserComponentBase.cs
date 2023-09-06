using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using JewelryApp.Models.Dtos;
using JewelryApp.Common.Enums;

namespace JewelryApp.Client.Pages.Components;

public abstract class UserComponentBase : ComponentBase
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private HttpClient? _authorizedHttpClient;
    private HttpClient? _unauthorizedHttpClient;
    private int _busyCounter = 0;
    private CancellationTokenSource? _cancellationTokenSource;

    protected bool IsBusy => _busyCounter > 0;
    protected CancellationTokenSource CancellationTokenSource
    {
        get
        {
            _cancellationTokenSource ??= new CancellationTokenSource();

            return _cancellationTokenSource;
        }
    }

    protected HttpClient AuthorizedHttpClient
    {
        get
        {
            _authorizedHttpClient ??= ClientFactory.CreateClient("AuthorizedClient");

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

    [Inject] private IHttpClientFactory ClientFactory { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
    [Inject] protected ISnackbar SnackBar { get; set; } = default!;

    public ValidationProblemDetails? ValidationProblems { get; private set; }

    protected UserComponentBase()
    {
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    protected override void OnInitialized()
    {
        if (ClientFactory is null)
            throw new InvalidOperationException("No ClientFactory is registered.");

        base.OnInitialized();
    }

    protected async Task<TResult?> GetAsync<TResult>(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            SetBusy(true);
            Reset();

            var response = await AuthorizedHttpClient.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

                return await JsonSerializer.DeserializeAsync<TResult>(stream, _jsonSerializerOptions, cancellationToken);
            }
            else
            {
                SnackBar.Add("دريافت اطلاعات با خطا مواجه شد", Severity.Warning);
                return default;
            }
        }
        catch (Exception)
        {
            // ignore
            return default;
        }
        finally
        {
            SetBusy(false);
        }
    }

    protected async Task PostAsync<TValue>(string url, TValue value, CancellationToken cancellationToken = default)
    {
        try
        {
            SetBusy(true);
            Reset();

            var response = await AuthorizedHttpClient.PostAsJsonAsync(url, value, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

                ValidationProblems = await JsonSerializer.DeserializeAsync<ValidationProblemDetails>(stream, _jsonSerializerOptions, cancellationToken);
            }
            else if (!response.IsSuccessStatusCode)
            {
                SnackBar.Add("خطا در ارسال اطلاعات", Severity.Error);
            }
            else
            {
                SnackBar.Add("عملیات با موفقیت انجام شد", Severity.Success);
            }
        }
        catch (Exception)
        {
            // ignore
        }
        finally
        {
            SetBusy(false);
        }
    }

    protected async Task PutAsync<TValue>(string url, TValue value, CancellationToken cancellationToken = default)
    {
        try
        {
            SetBusy(true);
            Reset();

            var response = await AuthorizedHttpClient.PutAsJsonAsync(url, value, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

                ValidationProblems = await JsonSerializer.DeserializeAsync<ValidationProblemDetails>(stream, _jsonSerializerOptions, cancellationToken);
            }
            else if (!response.IsSuccessStatusCode)
            {
                SnackBar.Add("خطا در ارسال اطلاعات", Severity.Error);
            }
            else
            {
                SnackBar.Add("عملیات با موفقیت انجام شد", Severity.Success);
            }
        }
        catch (Exception)
        {
            // ignore
        }
        finally
        {
            SetBusy(false);
        }
    }

    protected async Task DeleteAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            SetBusy(true);
            Reset();

            var response = await AuthorizedHttpClient.DeleteFromJsonAsync<DeleteResult>(url, cancellationToken);

            switch (response)
            {
                case DeleteResult.CanNotDelete:
                    SnackBar.Add("امکان حذف این جنس وجود ندارد", Severity.Warning);
                    break;
                case DeleteResult.IsNotAvailable:
                    SnackBar.Add("خطا در حذف جنس", Severity.Error);
                    break;
                case DeleteResult.Deleted:
                    SnackBar.Add("حذف جنس با موفقیت انجام شد", Severity.Error);
                    break;
                default:
                    break;
            }
        }
        catch (Exception)
        {
            // ignore
        }
        finally
        {
            SetBusy(false);
        }
    }

    protected void SetBusy(bool isBusy)
    {
        if (isBusy)
        {
            _busyCounter++;

            //if(_busyCounter == 1) // just got busy
            //    StateHasChanged();
        }
        else if (_busyCounter > 0)
        {
            _busyCounter--;

            //if (_busyCounter == 0) // just got ideal
            //    StateHasChanged();
        }
    }

    private void Reset()
    {
        ValidationProblems = null;
    }

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