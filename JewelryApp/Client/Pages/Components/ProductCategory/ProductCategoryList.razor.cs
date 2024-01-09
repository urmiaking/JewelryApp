using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.ProductCategories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.ProductCategory;

public partial class ProductCategoryList
{
    private List<ProductCategoryVm> _productCategories = new();
    private MudTable<ProductCategoryVm> _table = new();
    private string? _searchString;

    [Inject]
    public IProductCategoryService ProductCategoryService { get; set; } = default!;

    private async Task<TableData<ProductCategoryVm>> ServerReload(TableState state)
    {
        await LoadData(state);

        return new TableData<ProductCategoryVm> { TotalItems = _productCategories.Count, Items = _productCategories };
    }

    private async Task LoadData(TableState state)
    {
        var productCatergories = await ProductCategoryService.GetProductCategoriesAsync(CancellationTokenSource.Token);

        _productCategories = Mapper.Map<List<ProductCategoryVm>>(productCatergories);

        StateHasChanged();
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
