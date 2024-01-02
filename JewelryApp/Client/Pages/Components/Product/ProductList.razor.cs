using Microsoft.AspNetCore.Components;
using MudBlazor;
using JewelryApp.Client.Shared;
using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Requests.Products;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class ProductList
{
    [Inject] public IDialogService Dialog { get; set; } = default!;
    [Inject] public IProductService ProductService { get; set; } = default!;

    [Parameter] 
    public string? Class { get; set; }

    private List<ProductListVm> _products = new ();

    private HashSet<ProductListVm> _selectedItems = new();

    private DialogOptions _dialogOptions = new() { CloseButton = true, FullWidth = true, FullScreen = false };

    private MudTable<ProductListVm> _table = new();

    private int _totalItems;

    private string? _searchString;

    private async Task<TableData<ProductListVm>> ServerReload(TableState state)
    {
        await LoadData(state);

        return new TableData<ProductListVm> { TotalItems = _totalItems, Items = _products };
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
            if (result.Data is ProblemDetails validationProblems)
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
        var request = new GetProductsRequest(state.Page, state.PageSize, state.SortDirection.ToDescriptionString(),
            state.SortLabel, _searchString);

        var products = await ProductService.GetProductsAsync(request, CancellationTokenSource.Token);

        _products = Mapper.Map<List<ProductListVm>>(products);

        var itemCountResponse = await ProductService.GetTotalProductsCount(CancellationTokenSource.Token);

        _totalItems = itemCountResponse.Count;

        StateHasChanged();
    }

    private async Task CommitItemAsync(object item)
    {
        var product = item as ProductListVm;

        var request = Mapper.Map<UpdateProductRequest>(product);

        var response = await ProductService.UpdateProductAsync(request, CancellationTokenSource.Token);

        if (response.IsError)
        {
            foreach (var responseError in response.Errors)
            {
                SnackBar.Add(responseError.Description, Severity.Error);
            }
        }
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