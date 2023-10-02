using Microsoft.AspNetCore.Components;
using MudBlazor;
using JewelryApp.Client.Shared;
using JewelryApp.Models.Dtos.Product;
using JewelryApp.Models.Dtos.Common;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class ProductList
{
    [Inject] 
    public IDialogService Dialog { get; set; } = default!;

    [Parameter] 
    public string? Class { get; set; }

    private List<ProductTableItemDto> _products = new ();

    private HashSet<ProductTableItemDto> _selectedItems = new();

    private DialogOptions _dialogOptions = new() { CloseButton = true, FullWidth = true, FullScreen = false };

    private MudTable<ProductTableItemDto> _table = new();

    private int _totalItems;

    private string? _searchString;

    private async Task<TableData<ProductTableItemDto>> ServerReload(TableState state)
    {
        await LoadData(state);

        return new TableData<ProductTableItemDto> { TotalItems = _totalItems, Items = _products };
    }

    private void OnSearch(string? text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }

    private async Task OpenAddProductDialog(DialogOptions options, string dialogTitle)
    {
        var dialog = await Dialog.ShowAsync<AddProductDialog>(dialogTitle, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            if (result.Data is ValidationProblemDetails validationProblems)
            {
                if (validationProblems.Errors.Count > 0)
                {
                    foreach (var error in validationProblems.Errors)
                    {
                        SnackBar.Add(error.Value.FirstOrDefault(), Severity.Error);
                    }
                }
            }
            else
            {
                await _table.ReloadServerData();
            }
        }
    }

    private async Task LoadData(TableState state)
    {
        _products = await GetAsync<List<ProductTableItemDto>>(
                        $"/api/Products?page={state.Page}&pageSize={state.PageSize}&sortDirection={state.SortDirection}&sortLabel={state.SortLabel}&searchString={_searchString}")
                    ?? new List<ProductTableItemDto>();

        _totalItems = await GetAsync<int>("/api/Products/GetTotalProductsCount");

        StateHasChanged();
    }

    private async Task CommitItemAsync(object item)
    {
        var product = item as ProductTableItemDto;

        var productDto = new ProductDto
        {
            Id = product!.Id,
            Name = product.Name,
            Carat = product.Carat,
            ProductType = product.ProductType,
            Wage = product.Wage,
            Weight = product.Weight,
            Barcode = product.Barcode
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

            if (isDeleted)
            {
                await _table.ReloadServerData();
            }
        }
    }

    private void PageChanged(int i)
    {
        _table.NavigateTo(i - 1);
    }
}