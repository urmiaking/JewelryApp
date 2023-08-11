using System.Net.Http.Json;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Dashboard;

public partial class DashboardHeader
{

    private PriceDto? _priceDto;

    private Timer? _timer;

    [Parameter] public int RefreshInterval { get; set; } = 5;

    protected override async Task OnInitializedAsync()
    {
        await RefreshHeaderData();
        _timer = new Timer(async (_) => await RefreshHeaderData(), null, TimeSpan.Zero, TimeSpan.FromSeconds(RefreshInterval));
    }

    private async Task RefreshHeaderData()
    {
        try
        {
            _priceDto = await AuthorizedHttpClient.GetFromJsonAsync<PriceDto>("api/Price");
            StateHasChanged();
        }
        catch (Exception ex)
        {
            SnackBar.Add(ex.Message, Severity.Error);
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}