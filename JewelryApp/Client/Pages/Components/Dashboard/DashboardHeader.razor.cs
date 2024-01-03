using JewelryApp.Client.Services;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Responses.Prices;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace JewelryApp.Client.Pages.Components.Dashboard;

public partial class DashboardHeader
{
    [Inject] public SignalRService SignalRService { get; set; } = default!;
    [Inject] public IPriceService PriceService { get; set; } = default!;

    private PriceResponse? _model = default!;

    protected override async Task OnInitializedAsync()
    {
        var response = await PriceService.GetPriceAsync(CancellationTokenSource.Token);

        _model = response;

        await SignalRService.Connect();
        SignalRService.RegisterUpdateHandler(UpdatePriceValue);
    }

    private void UpdatePriceValue(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return;

        var response = JsonConvert.DeserializeObject<PriceResponse>(json);

        if (response is not null)
        {
            _model = response;

            StateHasChanged();
        }
    }
}