using JewelryApp.Common.Enums;
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

    public InvoiceDto InvoiceModel { get; set; } = new()
    {
        BuyDateTime = DateTime.Now,
        Products = new List<ProductDto>()
        {
            new ()
            {
                Count = 1,
                Caret = Caret.Eighteen,
                Index = 1,
                ProductType = ProductType.Gold,
                TaxOffset = 9
            }
        }
    };

    protected override async Task OnInitializedAsync()
    {
        if (Id is not null)
        {
            InvoiceModel = await GetAsync<InvoiceDto>($"/api/Invoices/{Id}") ?? throw new InvalidOperationException();

            var index = 1;
            foreach ( var item in InvoiceModel.Products)
            {
                item.Index = index;
                index++;
            }

        }
        else
        {
            var priceDto = await GetAsync<PriceDto>("api/Price");
            InvoiceModel.GramPrice = priceDto!.Gold18K;
        }
        
        await base.OnInitializedAsync();
    }

    public PatternMask NationalCodeMask = new("XXX-XXXXXX-X")
    {
        MaskChars = new[] { new MaskChar('X', @"[0-9]") },
        Placeholder = '_',
        CleanDelimiters = true
    };

    public PatternMask GramPriceMask = new("X,XXX,XXX")
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

    private int lastIndex = 1;

    private void AddRow()
    {
        lastIndex += 1;
        InvoiceModel.Products.Add(new ProductDto() { Index = lastIndex });
    }

    private void RemoveRow(ProductDto productItem)
    {
        lastIndex -= 1;
        InvoiceModel.Products.Remove(productItem);
    }

    private void ProductTypeChanged(ChangeEventArgs args, ProductDto context)
    {
        if (args.Value is not null)
        {
            var productTypeAsString = args.Value.ToString();

            var productType = Enum.Parse<ProductType>(productTypeAsString!);

            ChangeInvoiceModel(productType, InvoiceModel, context);
        }
    }

    private static void ChangeInvoiceModel(ProductType productType, InvoiceDto invoice, ProductDto context)
    {
        var product = invoice.Products.FirstOrDefault(a => a.Index == context.Index);

        context.ProductType = productType;
        product!.Profit = productType switch
        {
            ProductType.Jewelry => 20,
            ProductType.Gold => 7,
            _ => throw new ArgumentOutOfRangeException(nameof(productType), productType, null)
        };
    }

    private async Task Submit()
    {
        var validated = Validate();

        if (validated)
        {
            await PostAsync("api/Invoices", InvoiceModel);
        }
    }

    private bool Validate()
    {
        var errorList = new List<string>();
        if (!InvoiceModel.Products.Any())
        {
            errorList.Add("لطفا حداقل یک کالا در فاکتور وارد نمایید");
        }
        else
        {
            if (InvoiceModel.Products.Any(x => x.Weight == 0))
            {
                errorList.Add("وزن کالا نمی تواند صفر باشد");
            }

            if (InvoiceModel.Products.Any(x => string.IsNullOrEmpty(x.Name)))
            {
                errorList.Add("لطفا نام کالا را وارد نمایید");
            }
        }

        if (errorList.Any())
        {
            foreach (var error in errorList)
            {
                SnackBar.Add(error, Severity.Error);
            }

            return false;
        }

        return true;
    }
}

