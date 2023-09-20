using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using JewelryApp.Client.Shared;
using JewelryApp.Common.Enums;
using static MudBlazor.CategoryTypes;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class ProductList
{
    [Inject] public IDialogService Dialog { get; set; } = default!;

    private IEnumerable<ProductTableItemDto> _pagedData = new List<ProductTableItemDto>();
    private MudTable<ProductTableItemDto> _table = new ();

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
        }).ToList();
        totalItems = _products.Count;
        _products = state.SortLabel switch
        {
            "barcodetext_field" => _products.OrderByDirection(state.SortDirection, o => o.BarcodeText).ToList(),
            "name_field" => _products.OrderByDirection(state.SortDirection, o => o.Name).ToList(),
            "weight_field" => _products.OrderByDirection(state.SortDirection, o => o.Weight).ToList(),
            "wage_field" => _products.OrderByDirection(state.SortDirection, o => o.Wage).ToList(),
            "productType_field" => _products.OrderByDirection(state.SortDirection, o => o.ProductType).ToList(),
            "caret_field" => _products.OrderByDirection(state.SortDirection, o => o.Caret).ToList(),
            _ => _products
        };

        _pagedData = _products.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new TableData<ProductTableItemDto>() { TotalItems = totalItems, Items = _pagedData };
    }

    private void OnSearch(string text)
    {
        searchString = text;
        _table.ReloadServerData();
    }

    [Parameter]
    public string? Class { get; set; }

    private List<ProductTableItemDto>? _products;

    private HashSet<ProductTableItemDto> _selectedItems = new();

    DialogOptions _dialogOptions = new() { CloseButton = true, FullWidth = true, FullScreen = false };

    private async Task OpenAddProductDialog(DialogOptions options, string dialogTitle)
    {
        var dialog = await Dialog.ShowAsync<AddProductDialog>(dialogTitle, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            if (result.Data is ValidationProblemDetails validationProblems)
            {
                if (validationProblems is not null && validationProblems.Errors.Count > 0)
                {
                    foreach (var error in validationProblems.Errors)
                    {
                        SnackBar.Add(error.Value.FirstOrDefault()!.ToString(), Severity.Error);
                    }
                }
            }
            else
            {
                await _table.ReloadServerData();
            }
        }
    }

    private async Task LoadData()
    {
        _products = await GetAsync<List<ProductTableItemDto>>("/api/Products") ?? new List<ProductTableItemDto>();
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

        var dialog = await Dialog.ShowAsync<PromptDialog>("حذف جنس", parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var isDeleted = (bool)result.Data;

            if (!isDeleted)
            {
                SnackBar.Add("حذف با خطا مواجه شد");
            }
        }

        await _table.ReloadServerData();
    }
}

