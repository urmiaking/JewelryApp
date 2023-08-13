﻿using JewelryApp.Client.Pages.Components.Invoice;
using JewelryApp.Data.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace JewelryApp.Client.Pages;

public partial class SetInvoice
{
    [Parameter]
    public int? Id { get; set; }
    
    [Inject] public IDialogService Dialog { get; set; } = default!;

    public Invoice InvoiceModel { get; set; } = new() { BuyDateTime = DateTime.Now };

    public PatternMask NationalCodeMask = new("XXX-XXXXXX-X")
    {
        MaskChars = new[] { new MaskChar('X', @"[0-9]") },
        Placeholder = '_',
        CleanDelimiters = true
    };

    public PatternMask PhoneNumberMask = new("XXXX-XXX-XXXX")
    {
        MaskChars = new[] { new MaskChar('X', @"[0-9]") },
        Placeholder = '_',
        CleanDelimiters = true
    };

    public string BarcodeText { get; set; } = default!;

    private void AddRow()
    {
        InvoiceModel.InvoiceProducts.Add(new InvoiceProduct());
    }

    private void RemoveRow(InvoiceProduct productItem)
    {
        InvoiceModel.InvoiceProducts.Remove(productItem);
    }

    DialogOptions _dialogOptions = new() { CloseButton = true, FullWidth = true, FullScreen = false };

    private async Task OpenAddProductDialog(DialogOptions options, string dialogTitle)
    {
        var dialog = await Dialog.ShowAsync<SetInvoiceProduct>(dialogTitle, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var data = (InvoiceProduct)result.Data;

            InvoiceModel.InvoiceProducts.Add(data);
        }
    }
}
