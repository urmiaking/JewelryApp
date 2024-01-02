using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.Products;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class AddProductDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public AddProductVm Model { get; set; } = new();

    [Inject] private IProductService ProductService { get; set; } = default!;

    private bool _processing;

    private void Cancel() => MudDialog.Cancel();

    private async Task OnValidSubmit()
    {
        var request = Mapper.Map<AddProductRequest>(Model);

        var response = await ProductService.AddProductAsync(request, CancellationTokenSource.Token);

        if (response.IsError)
        {
            foreach (var error in response.Errors)
            {
                SnackBar.Add(error.Description);
            }
        }
        else
        {
            MudDialog.Close(DialogResult.Ok(response.Value));
        }

        _processing = false;
    }
}