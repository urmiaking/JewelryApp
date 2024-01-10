using Microsoft.AspNetCore.Components;
using MudBlazor;
using JewelryApp.Client.Shared;
using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Requests.Products;
using static MudBlazor.CategoryTypes;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class ProductList
{
    [Inject] public IDialogService Dialog { get; set; } = default!;
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

    private async Task OpenDeleteProductDialog(DialogOptions options, ProductListVm model)
    {
        var parameters = new DialogParameters<RemoveProductDialog>
        {
            { x => x.Id, model.Id },
            { x => x.Name, model.Name },
            { x => x.Barcode, model.Barcode }
        };

        var dialog = await Dialog.ShowAsync<RemoveProductDialog>("حذف جنس", parameters, options);

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

    private string SelectedRowClassFunc(ProductListVm row, int rowNumber)
    {
        if (row.Deleted)
        {
            return "deleted";
        }

        return string.Empty;
    }

    private async void RowEditPreview(object obj)
    {
        var row = (ProductListVm)obj;

        var response = await ProductCategoryService.GetProductCategoriesAsync(CancellationTokenSource.Token);

        row.ProductCategories = Mapper.Map<List<ProductCategoryVm>>(response);

        row.ProductCategory = row.ProductCategories.FirstOrDefault(x => x.Name == row.CategoryName) ??
                              new ProductCategoryVm { Id = 0, Name = "انتخاب کنید" };

        StateHasChanged();
    }
}