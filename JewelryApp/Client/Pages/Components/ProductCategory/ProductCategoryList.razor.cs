using JewelryApp.Client.Pages.Components.Product;
using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Common;
using JewelryApp.Shared.Requests.ProductCategories;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace JewelryApp.Client.Pages.Components.ProductCategory;

public partial class ProductCategoryList
{
    private List<ProductCategoryVm> _productCategories = new();
    private MudTable<ProductCategoryVm> _table = new();
    private string? _searchString;

    private DialogOptions _dialogOptions = new() { CloseButton = true, FullWidth = true, FullScreen = false };

    [Inject] public IProductCategoryService ProductCategoryService { get; set; } = default!;
    [Inject] public IDialogService Dialog { get; set; } = default!;

    private async Task<TableData<ProductCategoryVm>> ServerReload(TableState state)
    {
        await LoadData(state);

        return new TableData<ProductCategoryVm> { TotalItems = _productCategories.Count, Items = _productCategories };
    }

    private async Task LoadData(TableState state)
    {
        var productCategories = await ProductCategoryService.GetProductCategoriesAsync(CancellationTokenSource.Token);

        _productCategories = Mapper.Map<List<ProductCategoryVm>>(productCategories);

        StateHasChanged();
    }

    private async Task OpenAddProductCategoryDialog(DialogOptions options, string dialogTitle)
    {
        var dialog = await Dialog.ShowAsync<AddProductCategoryDialog>(dialogTitle, options);

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
                SnackBar.Add("دسته بندی با موفقیت درج شد", Severity.Success);
                await _table.ReloadServerData();
            }
        }
    }

    private async Task OpenDeleteProductCategoryDialog(DialogOptions options, int productId)
    {
        var parameters = new DialogParameters<RemoveProductCategoryDialog> { { x => x.Id, productId } };

        var dialog = await Dialog.ShowAsync<RemoveProductCategoryDialog>("حذف دسته بندی اجناس", parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var isDeleted = (bool)result.Data;

            if (isDeleted)
            {
                SnackBar.Add("دسته بندی مورد نظر با موفقیت حذف شد", Severity.Success);
                await _table.ReloadServerData();
            }
        }
    }

    private async Task CommitItemAsync(object item)
    {
        var product = item as ProductCategoryVm;

        var request = Mapper.Map<UpdateProductCategoryRequest>(product);

        var response = await ProductCategoryService.UpdateProductCategoryAsync(request, CancellationTokenSource.Token);

        if (response.IsError)
        {
            foreach (var responseError in response.Errors)
            {
                SnackBar.Add(responseError.Description, Severity.Error);
            }
        }
    }

    private void OnSearch(string? text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }

    private void PageChanged(int i)
    {
        _table.NavigateTo(i - 1);
    }
}
