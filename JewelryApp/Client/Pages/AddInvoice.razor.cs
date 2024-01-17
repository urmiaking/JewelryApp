using JewelryApp.Client.ViewModels.Invoice;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Responses.Prices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages;

public partial class AddInvoice
{
    [Inject] private IInvoiceService InvoiceService { get; set; } = default!;
    [Inject] private IProductService ProductService { get; set; } = default!;
    [Inject] private IPriceService PriceService { get; set; } = default!;

    private readonly AddCustomerVm _customerModel = new();
    private readonly AddInvoiceVm _invoiceModel = new();
    private readonly List<AddInvoiceItemVm> _items = new ();
    private PriceResponse? _price = default!;
    private string? _barcodeText;

    protected override async Task OnInitializedAsync()
    {
        var response = await InvoiceService.GetLastInvoiceNumber(CancellationTokenSource.Token);

        _price = await PriceService.GetPriceAsync(CancellationTokenSource.Token);

        _invoiceModel.InvoiceNumber = response.InvoiceNumber;

        await base.OnInitializedAsync();
    }

    public PatternMask PhoneNumberMask = new("XXXX-XXX-XXXX")
    {
        MaskChars = new[] { new MaskChar('X', "[0-9]") },
        Placeholder = '_',
        CleanDelimiters = true
    };

    private void GoBack()
    {
        NavigationManager.NavigateTo("/");
    }

    private async Task BarcodeTextChanged(string barcode)
    {
        if (!string.IsNullOrEmpty(barcode) && barcode.Length > 5)
        {
            var response = await ProductService.GetProductByBarcodeAsync(barcode, CancellationTokenSource.Token);

            if (response.IsError)
            {
                foreach (var error in response.Errors)
                {
                    SnackBar.Add(error.Description, Severity.Error);
                }
            }
            else
            {
                var invoiceItem = Mapper.Map<AddInvoiceItemVm>(response.Value);
                AddInvoiceItem(invoiceItem);
            }
        }
    }

    private void AddInvoiceItem(AddInvoiceItemVm? invoiceItem)
    {
        if (invoiceItem != null && _price != null)
        {
            invoiceItem.DollarPrice = _price.UsDollar;
            invoiceItem.GramPrice = _price.Gram18;
        }

        _items.Add(invoiceItem ?? new AddInvoiceItemVm());
    }
}