using JewelryApp.Shared.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class RemoveProductDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public int Id { get; set; }

    [Parameter]
    public string Name { get; set; } = default!;

    [Parameter]
    public string Barcode { get; set; } = default!;

    [Parameter]
    public bool DeletePermanently { get; set; }

    [Inject]
    private IProductService ProductService { get; set; } = default!;

    async Task Submit()
    {
        var response = await ProductService.RemoveProductAsync(Id, DeletePermanently, CancellationTokenSource.Token);

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