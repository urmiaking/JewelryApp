using Microsoft.AspNetCore.Components;
using MudBlazor;
using JewelryApp.Client.Shared;
using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Client.ViewModels.Invoice;
using JewelryApp.Client.ViewModels.Product;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class ProductList
{
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IProductService ProductService { get; set; } = default!;
    [Inject] private IProductCategoryService ProductCategoryService { get; set; } = default!;


    [Parameter] 
    public string? Class { get; set; }

    private List<ProductListVm> _products = new ();

    private HashSet<ProductListVm> _selectedItems = new();

    private DialogOptions _dialogOptions = new() { CloseButton = true, FullWidth = true, FullScreen = false };

    private MudTable<ProductListVm> _table = new();

    private int _totalItems;

    private string? _searchString;

    private int _selectedRowNumber = -1;

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
        var dialog = await DialogService.ShowAsync<AddProductDialog>(dialogTitle, options);

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
                SnackBar.Add("جنس با موفقیت درج شد", Severity.Success);
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

    private async Task RemoveRow(ProductListVm model)
    {
        var parameters = new DialogParameters<RemoveProductDialog>
        {
            { x => x.Id, model.Id },
            { x => x.Name, model.Name },
            { x => x.Barcode, model.Barcode }
        };

        var dialog = await DialogService.ShowAsync<RemoveProductDialog>("حذف جنس", parameters, _dialogOptions);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var isDeleted = (bool)result.Data;

            if (isDeleted)
            {
                SnackBar.Add("جنس مورد نظر با موفقیت حذف شد", Severity.Success);
                await _table.ReloadServerData();
            }
        }
    }

    private void PageChanged(int i)
    {
        _table.NavigateTo(i - 1);
    }

    private async Task EditRow(ProductListVm context)
    {
        var editProductVm = Mapper.Map<EditProductVm>(context);

        var parameters = new DialogParameters<EditProductDialog> { { x => x.Model, editProductVm } };

        var dialog = await DialogService.ShowAsync<EditProductDialog>("ویرایش جنس", parameters);

        var result = await dialog.Result;

        if (!result.Canceled)
            if (result.Data is EditProductVm data)
            {
                var item = Mapper.Map<ProductListVm>(data);
                _products.Remove(context);
                _products.Add(item);
            }
        StateHasChanged();
    }
}