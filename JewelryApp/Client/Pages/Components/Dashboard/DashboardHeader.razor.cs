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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await RefreshHeaderData();
            _timer = new Timer(async (_) => await RefreshHeaderData(), null, TimeSpan.Zero, TimeSpan.FromSeconds(RefreshInterval));
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task RefreshHeaderData()
    {
        try
        {
            _priceDto = await GetAsync<PriceDto>("api/Price");
            StateHasChanged();
        }
        catch
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}