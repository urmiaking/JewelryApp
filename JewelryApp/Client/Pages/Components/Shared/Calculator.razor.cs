using JewelryApp.Models.Dtos;

namespace JewelryApp.Client.Pages.Components.Shared;

public partial class Calculator : IDisposable
{
    private bool _isOpen;
    private ProductDto? _productDto = new();
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
        var priceDto = await GetAsync<PriceDto>("api/Price");

        _gramPrice = priceDto!.Gold18K;

        _productDto!.GramPrice = _gramPrice;
    }

    public void Dispose()
    {
        _productDto = new ProductDto
        {
            GramPrice = _gramPrice
        };
    }

    private async Task BarcodeChanged(string barcode)
    {
        if (!string.IsNullOrEmpty(barcode) && barcode.Length > 5)
        {
            _productDto = await GetAsync<ProductDto>($"api/Products/{barcode}");
            _productDto!.GramPrice = _gramPrice;

        }
    }
}