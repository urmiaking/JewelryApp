using JewelryApp.Shared.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class RemoveProductsDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public List<int> Ids { get; set; } = new();

    [Parameter] public List<string> Names { get; set; } = new();

    [Parameter]
    public bool DeletePermanently { get; set; }

    [Inject]
    private IProductService ProductService { get; set; } = default!;

    async Task Submit()
    {
        foreach (var id in Ids)
            await ProductService.RemoveProductAsync(id, DeletePermanently, CancellationTokenSource.Token);

        MudDialog.Close(DialogResult.Ok(true));
    }

    void Cancel() => MudDialog.Cancel();
}
