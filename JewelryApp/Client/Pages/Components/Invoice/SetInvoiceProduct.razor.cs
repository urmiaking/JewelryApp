using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Invoice;

public partial class SetInvoiceProduct
{
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public ProductDto Model { get; set; } = new();

    void Cancel() => MudDialog.Cancel();

    private void OnValidSubmit()
    {
        StateHasChanged();
    }

    void Submit()
    {
        var result = new InvoiceProduct
        {
            Product = new Data.Models.Product
            {
                Caret = Model.Caret,
                Wage = Model.Wage,
                ProductType = Model.ProductType,
                Name = Model.Name,
                Weight = Model.Weight
            },
            Count = Model.Count,
            Profit = Model.Profit,
            TaxOffset = Model.TaxOffset,
            GramPrice = Model.GramPrice
        };

        MudDialog.Close(DialogResult.Ok(result));
    }
}

