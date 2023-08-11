using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using JewelryApp.Client.Shared;
using JewelryApp.Common.Enums;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class ProductList
{
    [Inject] public IDialogService Dialog { get; set; } = default!;

    private IEnumerable<ProductTableItemDto> pagedData;
    private MudTable<ProductTableItemDto> table;

    private int totalItems;
    private string searchString = null;

    private async Task<TableData<ProductTableItemDto>> ServerReload(TableState state)
    {
        await LoadData();
        _products = _products.Where(product =>
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (product.BarcodeText.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (product.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{product.Weight} {product.Wage} {product.ProductType.ToDisplay()} {product.Caret.ToDisplay()}".Contains(searchString))
                return true;
            return false;
        }).ToArray();
        totalItems = _products.Count();
        switch (state.SortLabel)
        {
            case "barcodetext_field":
                _products = _products.OrderByDirection(state.SortDirection, o => o.BarcodeText);
                break;
            case "name_field":
                _products = _products.OrderByDirection(state.SortDirection, o => o.Name);
                break;
            case "weight_field":
                _products = _products.OrderByDirection(state.SortDirection, o => o.Weight);
                break;
            case "wage_field":
                _products = _products.OrderByDirection(state.SortDirection, o => o.Wage);
                break;
            case "productType_field":
                _products = _products.OrderByDirection(state.SortDirection, o => o.ProductType);
                break;
            case "caret_field":
                _products = _products.OrderByDirection(state.SortDirection, o => o.Caret);
                break;
        }

        pagedData = _products.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new TableData<ProductTableItemDto>() { TotalItems = totalItems, Items = pagedData };
    }

    private void OnSearch(string text)
    {
        searchString = text;
        table.ReloadServerData();
    }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public bool Filterable { get; set; }
    
    private IEnumerable<ProductTableItemDto>? _products;


    DialogOptions _dialogOptions = new() { CloseButton = true, FullWidth = true, FullScreen = false };

    private async Task OpenAddProductDialog(DialogOptions options, string dialogTitle)
    {
        await Dialog.ShowAsync<AddProductDialog>(dialogTitle, options);
        await table.ReloadServerData();
    }

    private async Task LoadData()
    {
        _products = await GetAsync<IEnumerable<ProductTableItemDto>>("/api/Products");
        StateHasChanged();
    }

    private async Task CommitItemAsync(object elemnt)
    {
        var product = elemnt as ProductTableItemDto;

        var productDto = new SetProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Caret = product.Caret,
            ProductType = product.ProductType,
            Wage = product.Wage,
            Weight = product.Weight,
            BarcodeText = product.BarcodeText
        };

        await PostAsync("/api/Products", productDto);
    }

    private async Task OpenDeleteProductDialog(DialogOptions options, int productId)
    {
        var parameters = new DialogParameters<PromptDialog>
        {
            { x => x.ContentText, "آیا مطمئن هستید؟ تنها اجناسی قابل حذف می باشند که در فاکتوری استفاده نشده باشند" },
            { x => x.ButtonText, "حذف" },
            { x => x.Color, Color.Error },
            { x => x.EndIcon, Icons.Material.Filled.Delete },
            { x => x.EndpointUrl, $"/api/Products/{productId}"}
        };

        await Dialog.ShowAsync<PromptDialog>("حذف جنس", parameters, options);
        await LoadData();
    }
}

