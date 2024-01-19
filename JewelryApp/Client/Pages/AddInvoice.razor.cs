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
    }

    private void RemoveRow(AddInvoiceItemVm item)
    {
        _items.Remove(item);
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
    }

    private bool _isDiscountOpen;
    private string? discountText;
    private void ToggleDiscount()
    {
        //TODO: Find a solution
        discountText = _invoiceModel.Discount.HasValue ? _invoiceModel.Discount.Value.ToCurrency() : "ندارد";
        _isDiscountOpen = !_isDiscountOpen;
        StateHasChanged();
    }

    private bool _isDebtOpen;
    private string? debtText;
    private void ToggleDebt()
    {
        //TODO: Find a solution
        debtText = _invoiceModel.Debt.HasValue ? _invoiceModel.Debt.Value.ToCurrency() : "ندارد";
        _isDebtOpen = !_isDebtOpen;
        StateHasChanged();
    }
}