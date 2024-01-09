using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.ProductCategories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.ProductCategory;

public partial class AddProductCategoryDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = default!;

    private ProductCategoryVm _model = new();

    [Inject] private IProductCategoryService ProductCategoryService { get; set; } = default!;

    private bool _processing;
    private void Cancel() => MudDialog.Cancel();
    private async Task OnValidSubmit()
    {
        _processing = true;

        var request = Mapper.Map<AddProductCategoryRequest>(_model);

        var response = await ProductCategoryService.AddProductCategoryAsync(request, CancellationTokenSource.Token);

        if (response.IsError)
            foreach (var error in response.Errors)
                SnackBar.Add(error.Description, Severity.Error);
        else
            MudDialog.Close(DialogResult.Ok(response.Value));

        _processing = false;
    }
}
