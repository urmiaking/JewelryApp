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

    private async Task OnValidSubmit()
    {
        await PostAsync("api/Products", Model);

        _processing = false;

        if (ValidationProblems != null && ValidationProblems.Errors.Count > 0)
        {
            MudDialog.Close(DialogResult.Ok(ValidationProblems));
        }
        else
        {
            MudDialog.Close(DialogResult.Ok(1));
        }
    }
}