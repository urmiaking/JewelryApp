using JewelryApp.Shared.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.ProductCategory;

public partial class RemoveProductCategoryDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public int Id { get; set; }

    [Parameter]
    public bool DeletePermanently { get; set; }

    [Inject]
    private IProductCategoryService ProductCategoryService { get; set; } = default!;

    async Task Submit()
    {
        var response = await ProductCategoryService.RemoveProductCategoryAsync(Id, DeletePermanently, CancellationTokenSource.Token);

        if (response.IsError)
        {
            foreach (var responseError in response.Errors)
            {
                SnackBar.Add(responseError.Description, Severity.Error);
            }
        }
        else
        {
            MudDialog.Close(DialogResult.Ok(true));
        }
    }

    void Cancel() => MudDialog.Cancel();
}