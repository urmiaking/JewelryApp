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
        Products = new List<InvoiceItemDto>
        {
            new ()
            {
                Quantity = 1,
                Index = 1,
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

    public string? BarcodeText { get; set; }

    private int _lastIndex = 1;

    private void AddRow()
    {
        _lastIndex += 1;
        InvoiceModel.Products.Add(new InvoiceItemDto { Index = _lastIndex });
    }

    private void AddRow(InvoiceItemDto invoiceItem)
    {
        _lastIndex += 1;
        invoiceItem.Index = _lastIndex;
        InvoiceModel.Products.Add(invoiceItem);
    }

    private void RemoveRow(InvoiceItemDto invoiceItemItem)
    {
        if (_lastIndex >= 1)
        {
            _lastIndex -= 1;
        }
        
        InvoiceModel.Products.Remove(invoiceItemItem);
    }

    private void ProductTypeChanged(ChangeEventArgs args, InvoiceItemDto context)
    {
        if (args.Value is not null)
        {
            var productTypeAsString = args.Value.ToString();

            var productType = Enum.Parse<ProductType>(productTypeAsString!);

            ChangeInvoiceModel(productType, InvoiceModel, context);
        }
    }

    private static void ChangeInvoiceModel(ProductType productType, InvoiceDto invoice, InvoiceItemDto context)
    {
        var product = invoice.Products.FirstOrDefault(a => a.Index == context.Index);

        context.Product.ProductType = productType;
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
            if (InvoiceModel.Products.Any(x => x.Product.Weight == 0))
            {
                errorList.Add("وزن کالا نمی تواند صفر باشد");
            }

            if (InvoiceModel.Products.Any(x => string.IsNullOrEmpty(x.Product.Name)))
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

    private void GoBack()
    {
        NavigationManager.NavigateTo("/");
    }

    private async Task BarcodeChanged(string barcode)
    {
        if (!string.IsNullOrEmpty(barcode) && barcode.Length > 5)
        {
            var product = await GetAsync<InvoiceItemDto>($"api/Products/{barcode}");

            if (product is not null)
            {
                var emptyRow = InvoiceModel.Products.FirstOrDefault(x => string.IsNullOrEmpty(x.Product.Name));
                if (emptyRow is not null)
                {
                    RemoveRow(emptyRow);
                }

                AddRow(product);
            }
        }
    }
}

