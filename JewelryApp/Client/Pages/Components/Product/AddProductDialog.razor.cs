using JewelryApp.Client.Services;
using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Products;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class AddProductDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = default!;
    [Inject] private IProductService ProductService { get; set; } = default!;
    [Inject] private IProductCategoryService ProductCategoryService { get; set; } = default!;

    private AddProductVm _model = new();
    private IEnumerable<GetProductResponse>? _inputProducts = new List<GetProductResponse>();
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

    async Task HandleNameChange(ChangeEventArgs e)
    {
        var name = e.Value?.ToString();
        if (name?.Length > 2)
        {
            _inputProducts = await ProductService.GetProductsByNameAsync(name, CancellationTokenSource.Token);
        }
        else
        {
            _inputProducts = null;
        }
    }

    void SelectProduct(GetProductResponse selectedProduct)
    {
        var productCategories = _model.ProductCategories;
        _model = Mapper.Map<AddProductVm>(selectedProduct);
        _model.ProductCategories = productCategories;
        _model.ProductCategory = _model.ProductCategories.FirstOrDefault(c => c.Name.Equals(selectedProduct.Name)) ?? _model.ProductCategories.First();
        _model.Barcode = BarcodeService.IncrementByOne(_model.Barcode);
        _inputProducts = null;
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