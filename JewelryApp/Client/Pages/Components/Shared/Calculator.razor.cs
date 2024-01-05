using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JewelryApp.Client.Pages.Components.Shared;

public partial class Calculator : IDisposable
{
    [Inject] private IPriceService PriceService { get; set; } = default!;
    [Inject] private IProductService ProductService { get; set; } = default!;

    private bool _isOpen;
    private bool _isBusy;
    private CalculatorVm _model = new();

    public async Task ToggleOpen()
    {
        _isOpen = !_isOpen;
        if (!_isOpen)
        {
            Dispose();
        }
        else
        {
            await GetPrice();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await GetPrice();

        await base.OnInitializedAsync();
    }

    private async Task GetPrice()
    {
        var response = await PriceService.GetPriceAsync(CancellationTokenSource.Token);

        if (response == null)
            return;

        _model.GramPrice = response.Gram18;
        _model.DollarPrice = response.UsDollar;
    }

    public void Dispose()
    {
        _model = new();
    }

    private async Task BarcodeChanged(string barcode)
    {
        if (!string.IsNullOrEmpty(barcode) && barcode.Length > 5)
        {
            _isBusy = true;
            
            var response = await ProductService.GetProductByBarcodeAsync(barcode, CancellationTokenSource.Token);

            if (!response.IsError)
            {
                _model = Mapper.Map<CalculatorVm>(response.Value);
                _model.Barcode = string.Empty;
                await GetPrice();
            }
            _isBusy = false;
            StateHasChanged();
        }
    }
}