using JewelryApp.Client.Services;
using JewelryApp.Client.ViewModels;
using JewelryApp.Client.ViewModels.Product;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Products;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class EditProductDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = default!;
    [Inject] private IProductService ProductService { get; set; } = default!;
    [Inject] private IProductCategoryService ProductCategoryService { get; set; } = default!;
    [Parameter] public EditProductVm Model { get; set; } = default!;

    private IEnumerable<GetProductResponse>? _inputProducts = new List<GetProductResponse>();
    private bool _processing;

    protected override async Task OnInitializedAsync()
    {
        var response = await ProductCategoryService.GetProductCategoriesAsync(CancellationTokenSource.Token);

        Model.ProductCategories = Mapper.Map<List<ProductCategoryVm>>(response);

        Model.ProductCategory = Model.ProductCategories.First(x => x.Name == Model.CategoryName);
    }

    private void Cancel() => MudDialog.Cancel();

    private void GenerateBarcode()
    {
        Model.Barcode = BarcodeService.Generate();
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
        var productCategories = Model.ProductCategories;
        Model = Mapper.Map<EditProductVm>(selectedProduct);
        Model.ProductCategories = productCategories;
        Model.ProductCategory = Model.ProductCategories.FirstOrDefault(c => c.Name.Equals(selectedProduct.Name)) ?? Model.ProductCategories.First();
        Model.Barcode = BarcodeService.IncrementByOne(Model.Barcode);
        _inputProducts = null;
    }

    private async Task OnValidSubmit()
    {
        _processing = true;

        var request = Mapper.Map<UpdateProductRequest>(Model);

        var response = await ProductService.UpdateProductAsync(request, CancellationTokenSource.Token);

        if (response.IsError)
            foreach (var error in response.Errors)
                SnackBar.Add(error.Description, Severity.Error);
        else
            MudDialog.Close(DialogResult.Ok(Model));

        _processing = false;
    }
}