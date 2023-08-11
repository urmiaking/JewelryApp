using System.Net.Http.Json;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Product;

public partial class AddProductDialog
{
    [CascadingParameter]
    public MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public SetProductDto Model { get; set; } = new();

    private bool _processing;

    void Cancel() => MudDialog.Cancel();

    private void OnValidSubmit()
    {
        StateHasChanged();
    }

    async Task Submit()
    {
        _processing = true;

        var httpResponseMessage = await AuthorizedHttpClient.PostAsJsonAsync("api/Products", Model);

        _processing = false;

        if (!httpResponseMessage.IsSuccessStatusCode)
            MudDialog.Close(DialogResult.Ok(false));

        MudDialog.Close(DialogResult.Ok(true));
    }

}

