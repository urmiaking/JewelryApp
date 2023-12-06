using JewelryApp.Client.Services;
using JewelryApp.Models.Dtos.Price;
using JewelryApp.Models.Dtos.PriceDtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;

namespace JewelryApp.Client.Pages.Components.Dashboard;

public partial class DashboardHeader
{
    [Inject] 
    public SignalRService SignalRService { get; set; } = default!;

    private PriceDto? _priceDto;

    protected override async Task OnInitializedAsync()
    {
        _priceDto = await GetAsync<PriceDto>("api/Price");
        await SignalRService.Connect();
        SignalRService.RegisterUpdateHandler(UpdatePriceValue);
    }

    private void UpdatePriceValue(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            SnackBar.Add("دریافت اطلاعات با خطا مواجه شد", Severity.Error);
            return;
        }

        _priceDto = JsonConvert.DeserializeObject<PriceDto>(json) ?? throw new InvalidDataException("Invalid Format of Received Data");
        
        StateHasChanged();
    }
}