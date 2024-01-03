using JewelryApp.Client.ViewModels;
using Microsoft.AspNetCore.Components.Web;

namespace JewelryApp.Client.Pages.Components.Shared;

public partial class Calculator : IDisposable
{
    private bool _isOpen;
    private CalculatorVm _model = new();
    public string? BarcodeText { get; set; }
    private double _gramPrice;

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
        //try
        //{
        //    var priceDto = await GetAsync<PriceDto>("api/Price");

        //    _gramPrice = priceDto!.Gold18K;

        //    _model.GramPrice = _gramPrice;
        //}
        //catch
        //{
        //    //Ignore
        //}
    }

    public void Dispose()
    {
        //_model = new ProductCalculationDto
        //{
        //    GramPrice = _gramPrice
        //};
    }

    private async Task BarcodeChanged(string barcode)
    {
        //if (!string.IsNullOrEmpty(barcode) && barcode.Length > 5)
        //{
        //    _model = await GetAsync<ProductCalculationDto>($"api/Products/{barcode}") ?? new ProductCalculationDto();
        //    _model.GramPrice = _gramPrice;

        //}
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