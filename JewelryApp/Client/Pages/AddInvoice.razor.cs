using JewelryApp.Client.Extensions;
using JewelryApp.Client.Pages.Components.Invoice;
using JewelryApp.Client.Pages.Components.Product;
using JewelryApp.Client.ViewModels.Invoice;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Responses.Prices;
using JewelryApp.Shared.Responses.Products;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages;

public partial class AddInvoice
{
    [Inject] private IInvoiceService InvoiceService { get; set; } = default!;
    [Inject] private IProductService ProductService { get; set; } = default!;
    [Inject] private IPriceService PriceService { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;

    private readonly AddCustomerVm _customerModel = new();
    private readonly AddInvoiceVm _invoiceModel = new();
    private readonly List<AddInvoiceItemVm> _items = new();
    private readonly List<AddOldGoldVm> _oldGoldItems = new();
    private PriceResponse? _price;
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
                await AddInvoiceItem(invoiceItem);
            }
        }
    }

    private async Task AddInvoiceItem(AddInvoiceItemVm? invoiceItem)
    {
        if (invoiceItem is null)
        {
            var dialog = await DialogService.ShowAsync<AddProductDialog>();

            var result = await dialog.Result;

            if (!result.Canceled)
                if (result.Data is AddProductResponse data)
                    invoiceItem = Mapper.Map<AddInvoiceItemVm>(data);
                else
                    return;
            else
                return;
        }

        if (invoiceItem != null && _price != null)
        {
            if (_items.Any(x => x.Id == invoiceItem.Id))
            {
                SnackBar.Add($"جنس با بارکد {invoiceItem.Barcode} قبلا به لیست اجناس افزوده شده است", Severity.Info);
                return;
            }

            invoiceItem.DollarPrice = _price.UsDollar;
            invoiceItem.GramPrice = _price.Gram18;
        }

        _items.Add(invoiceItem ?? new AddInvoiceItemVm());
        StateHasChanged();
    }

    private void RemoveRow(AddInvoiceItemVm item)
    {
        _items.Remove(item);
        StateHasChanged();
    }

    private async Task EditRow(AddInvoiceItemVm context)
    {
        var editInvoiceItemVm = Mapper.Map<EditInvoiceItemVm>(context);

        var parameters = new DialogParameters<EditInvoiceItem> { { x => x.Model, editInvoiceItemVm } };

        var dialog = await DialogService.ShowAsync<EditInvoiceItem>("ویرایش جنس", parameters);

        var result = await dialog.Result;

        if (!result.Canceled)
            if (result.Data is EditInvoiceItemVm data)
            {
                var item = Mapper.Map<AddInvoiceItemVm>(data);
                _items.Remove(context);
                _items.Add(item);
            }
        StateHasChanged();
    }

    private bool _isDiscountOpen;
    private void ToggleDiscount()
    {
        //TODO: Find a solution
        _isDiscountOpen = !_isDiscountOpen;
        StateHasChanged();
    }

    private bool _isDebtOpen;
    private void ToggleDebt()
    {
        //TODO: Find a solution
        _isDebtOpen = !_isDebtOpen;
        StateHasChanged();
    }

    private bool _isAdditionalPricesOpen;
    private void ToggleAdditionalPrices()
    {
        //TODO: Find a solution
        _isAdditionalPricesOpen = !_isAdditionalPricesOpen;
        StateHasChanged();
    }

    private bool _isOldGoldOpen;
    private void ToggleOldGold()
    {
        //TODO: Find a solution
        _isOldGoldOpen = !_isOldGoldOpen;
        StateHasChanged();
    }

    private void AddOldGold()
    {
        _oldGoldItems.Add(new AddOldGoldVm());
    }

    private void RemoveOldGoldRow(AddOldGoldVm context)
    {
        _oldGoldItems.Remove(context);
    }

    private void EditOldGoldRow(AddOldGoldVm context)
    {
        throw new NotImplementedException();
    }
}