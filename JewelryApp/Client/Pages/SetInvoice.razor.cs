using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages;

public partial class SetInvoice
{
    [Parameter]
    public int? Id { get; set; }
    
    [Inject] public IDialogService Dialog { get; set; } = default!;

    public InvoiceDto InvoiceModel { get; set; } = new() { BuyDateTime = DateTime.Now };

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

    private int lastIndex = 0;

    private void AddRow()
    {
        lastIndex += 1;
        InvoiceModel.Products.Add(new ProductDto() { Index = lastIndex});
    }

    private void RemoveRow(ProductDto productItem)
    {
        lastIndex -= 1;
        InvoiceModel.Products.Remove(productItem);
    }
}

