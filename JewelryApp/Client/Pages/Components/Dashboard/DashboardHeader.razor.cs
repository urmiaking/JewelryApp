using JewelryApp.Client.Services;
using JewelryApp.Shared.Responses.Prices;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace JewelryApp.Client.Pages.Components.Dashboard;

public partial class DashboardHeader
{
    [Inject] public SignalRService SignalRService { get; set; } = default!;

    private PriceResponse _priceDto = default!;

    protected override async Task OnInitializedAsync()
    {
        await SignalRService.Connect();
        SignalRService.RegisterUpdateHandler(UpdatePriceValue);
    }

    private void UpdatePriceValue(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return;

        _priceDto = JsonConvert.DeserializeObject<PriceResponse>(json) ?? default!;
        
        StateHasChanged();
    }
}