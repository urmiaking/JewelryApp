using JewelryApp.Client.Services;
using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.Products;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class AddProductDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = default!;

    private AddProductVm _model = new();

    [Inject] private IProductService ProductService { get; set; } = default!;
    [Inject] private IProductCategoryService ProductCategoryService { get; set; } = default!;

    private bool _processing;

    protected override async Task OnInitializedAsync()
    {
        GenerateBarcode();

        var response = await ProductCategoryService.GetProductCategoriesAsync(CancellationTokenSource.Token);

        _model.ProductCategories = Mapper.Map<List<ProductCategoryVm>>(response);
    }

    private void Cancel() => MudDialog.Cancel();

    private void GenerateBarcode()
    {
        _model.Barcode = BarcodeService.Generate();
    }

    private async Task OnValidSubmit()
    {
        _processing = true;

        var request = Mapper.Map<AddProductRequest>(_model);

        var response = await ProductService.AddProductAsync(request, CancellationTokenSource.Token);

        if (response.IsError)
            foreach (var error in response.Errors)
                SnackBar.Add(error.Description, Severity.Error);
        else
            MudDialog.Close(DialogResult.Ok(response.Value));

        _processing = false;
    }
}