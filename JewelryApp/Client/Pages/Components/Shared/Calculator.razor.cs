using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JewelryApp.Client.Pages.Components.Shared;

public partial class Calculator : IDisposable
{
    [Inject] private IPriceService PriceService { get; set; } = default!;
    [Inject] private IProductService ProductService { get; set; } = default!;

    private bool _isOpen;
    private CalculatorVm _model = new();
    public string? BarcodeText { get; set; }

    public void ToggleOpen()
    {
        _isOpen = !_isOpen;
        if (!_isOpen)
        {
            Dispose();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await GetGramPrice();

        await base.OnInitializedAsync();
    }

    private async Task GetGramPrice()
    {
        var response = await PriceService.GetPriceAsync(CancellationTokenSource.Token);

        if (response == null)
            return;

        _model.GramPrice = response.Gram18;
    }

    public void Dispose()
    {
        _model = default!;
    }

    private async Task BarcodeChanged(string barcode)
    {
        if (!string.IsNullOrEmpty(barcode) && barcode.Length > 5)
        {
            var response = await ProductService.GetProductByBarcodeAsync(barcode, CancellationTokenSource.Token);

            if (!response.IsError)
            {
                _model = Mapper.Map<CalculatorVm>(response.Value);
                await GetGramPrice();
            }
        }
    }

    private void WeightKeyHandler(KeyboardEventArgs e)
    {
        if (e.Key == "+")
        {
            _model.Weight += 1;
        }
        else if (e.Key == "-")
        {
            _model.Weight -= 1;
        }
    }
}