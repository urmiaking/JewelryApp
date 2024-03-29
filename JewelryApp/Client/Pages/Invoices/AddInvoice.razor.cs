﻿using ErrorOr;
using JewelryApp.Client.Pages.Components.Invoice;
using JewelryApp.Client.Pages.Components.Product;
using JewelryApp.Client.ViewModels.Invoice;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.Customer;
using JewelryApp.Shared.Requests.InvoiceItems;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Requests.OldGolds;
using JewelryApp.Shared.Responses.Customer;
using JewelryApp.Shared.Responses.InvoiceItems;
using JewelryApp.Shared.Responses.Invoices;
using JewelryApp.Shared.Responses.OldGolds;
using JewelryApp.Shared.Responses.Prices;
using JewelryApp.Shared.Responses.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace JewelryApp.Client.Pages.Invoices;

public partial class AddInvoice
{
    [Inject] private IInvoiceService InvoiceService { get; set; } = default!;
    [Inject] private IInvoiceItemService InvoiceItemService { get; set; } = default!;
    [Inject] private ICustomerService CustomerService { get; set; } = default!;
    [Inject] private IProductService ProductService { get; set; } = default!;
    [Inject] private IPriceService PriceService { get; set; } = default!;
    [Inject] private IOldGoldService OldGoldService { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;

    private AddCustomerVm _customerModel = new();
    private readonly AddInvoiceVm _invoiceModel = new();
    private readonly List<AddInvoiceItemVm> _items = new();
    private readonly List<AddOldGoldVm> _oldGoldItems = new();
    private PriceResponse? _price;
    private bool _processing;

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

        var parameters = new DialogParameters<EditInvoiceItemDialog> { { x => x.Model, editInvoiceItemVm } };

        var dialog = await DialogService.ShowAsync<EditInvoiceItemDialog>("ویرایش جنس", parameters);

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

    private async Task AddOldGold()
    {
        var parameters = new DialogParameters<AddOldGoldDialog> { { x => x.Model, new AddOldGoldVm { GramPrice = _price!.Gram18 } } };

        var dialog = await DialogService.ShowAsync<AddOldGoldDialog>("افزودن طلای کهنه", parameters);
        var result = await dialog.Result;

        if (!result.Canceled)
            if (result.Data is AddOldGoldVm item)
            {
                _oldGoldItems.Add(item);
            }
        StateHasChanged();
    }

    private void RemoveOldGoldRow(AddOldGoldVm context)
    {
        _oldGoldItems.Remove(context);
    }

    private async Task EditOldGoldRow(AddOldGoldVm context)
    {
        var parameters = new DialogParameters<AddOldGoldDialog> { { x => x.Model, context } };

        var dialog = await DialogService.ShowAsync<AddOldGoldDialog>("ویرایش طلای کهنه", parameters);
        var result = await dialog.Result;

        if (!result.Canceled)
            if (result.Data is AddOldGoldVm item)
            {
                _oldGoldItems.Remove(context);
                _oldGoldItems.Add(item);
            }
        StateHasChanged();
    }

    private async Task SaveInvoice()
    {
        if (string.IsNullOrEmpty(_customerModel.Name) || _invoiceModel.InvoiceNumber == 0 || !_invoiceModel.BuyDateTime.HasValue || !_items.Any())
        {
            SnackBar.Add("لطفا اطلاعات فاکتور را کامل کرده و سپس ذخیره نمایید", Severity.Error); return;
        }

        _processing = true;
        // 1- Ensure all data is validated

        ErrorOr<AddCustomerResponse> customerResponse = default;
        ErrorOr<AddInvoiceResponse> invoiceResponse = default;
        List<ErrorOr<AddInvoiceItemResponse>> invoiceItemsResponseList = new();
        List<ErrorOr<AddOldGoldResponse>> oldGoldsResponseList = new();

        try
        {
            // 2- Add Customer
            var customer = Mapper.Map<AddCustomerRequest>(_customerModel);

            customerResponse = await CustomerService.AddCustomerAsync(customer, CancellationTokenSource.Token);

            if (customerResponse.IsError)
            {
                foreach (var error in customerResponse.Errors)
                {
                    SnackBar.Add(error.Description);
                }

                return;
            }

            SnackBar.Add("اطلاعات مشتری اضافه شد");

            // 3- Add Invoice
            _invoiceModel.CustomerId = customerResponse.Value.Id;
            var invoice = Mapper.Map<AddInvoiceRequest>(_invoiceModel);

            invoiceResponse = await InvoiceService.AddInvoiceAsync(invoice, CancellationTokenSource.Token);

            if (invoiceResponse.IsError)
            {
                await RollBack(customerResponse.Value.Id);

                foreach (var error in invoiceResponse.Errors)
                {
                    SnackBar.Add(error.Description);
                }

                return;
            }

            SnackBar.Add("فاکتور اضافه شد");

            // 4- Add InvoiceItems

            foreach (var item in _items)
                item.InvoiceId = invoiceResponse.Value.Id;

            var invoiceItems = Mapper.Map<List<AddInvoiceItemRequest>>(_items);

            foreach (var invoiceItem in invoiceItems)
            {
                var invoiceItemsResponse =
                    await InvoiceItemService.AddInvoiceItemAsync(invoiceItem, CancellationTokenSource.Token);

                if (!invoiceItemsResponse.IsError)
                    invoiceItemsResponseList.Add(invoiceItemsResponse);

                else
                {
                    SnackBar.Add(invoiceItemsResponse.FirstError.Description, Severity.Error);
                    await RollBack(customerResponse.Value.Id, invoiceResponse.Value.Id, invoiceItemsResponseList);
                    return;
                }
            }

            SnackBar.Add("اجناس فاکتور اضافه شد");

            // 5- Add Old Golds (if any)

            if (_oldGoldItems.Any())
            {
                foreach (var oldGoldVm in _oldGoldItems)
                    oldGoldVm.InvoiceId = invoiceResponse.Value.Id;

                var oldGoldItems = Mapper.Map<List<AddOldGoldRequest>>(_oldGoldItems);

                foreach (var request in oldGoldItems)
                {
                    var oldGoldResponse = await OldGoldService.AddOldGoldAsync(request, CancellationTokenSource.Token);

                    if (!oldGoldResponse.IsError)
                        oldGoldsResponseList.Add(oldGoldResponse);

                    else
                    {
                        SnackBar.Add(oldGoldResponse.FirstError.Description, Severity.Error);
                        await RollBack(customerResponse.Value.Id, invoiceResponse.Value.Id, invoiceItemsResponseList,
                            oldGoldsResponseList);
                        return;
                    }
                }

                SnackBar.Add("طلاهای کهنه اضافه شدند");
            }
        }
        catch (Exception e)
        {
            SnackBar.Add(e.Message, Severity.Error);

            await RollBack(customerResponse.Value.Id, invoiceResponse.Value.Id, invoiceItemsResponseList,
                oldGoldsResponseList);
        }
        finally
        {
            _processing = false;
        }
        
    }

    private async Task RollBack(int? customerId = 0, int? invoiceId = 0, List<ErrorOr<AddInvoiceItemResponse>>? invoiceItems = default,
        List<ErrorOr<AddOldGoldResponse>>? oldGolds = default)
    {
        if (customerId.HasValue)
        {
            await CustomerService.RemoveCustomerAsync(customerId.Value, true, CancellationTokenSource.Token);
        }

        if (invoiceId.HasValue)
        {
            await InvoiceService.RemoveInvoiceAsync(invoiceId.Value, true, CancellationTokenSource.Token);
        }

        if (invoiceItems is not null)
            foreach (var invoiceItemId in invoiceItems)
                await InvoiceItemService.RemoveInvoiceItemAsync(invoiceItemId.Value.Id, true, CancellationTokenSource.Token);

        if (oldGolds is not null)
            foreach (var oldGoldId in oldGolds)
                await OldGoldService.RemoveOldGoldAsync(oldGoldId.Value.Id, true, CancellationTokenSource.Token);
    }

    private async Task CheckNationalCode(KeyboardEventArgs arg)
    {
        if (arg.Code is "NumpadEnter" or "Enter")
        {
            var customerResponse =
                await CustomerService.GetCustomerByNationalCodeAsync(_customerModel.NationalCode ?? string.Empty,
                    CancellationTokenSource.Token);

            if (!customerResponse.IsError)
            {
                var customerVm = Mapper.Map<AddCustomerVm>(customerResponse.Value);
                _customerModel = customerVm;
            }
        }
    }
}