using JewelryApp.Client.ViewModels.Invoice;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Invoice;

public partial class AddOldGoldDialog
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public AddOldGoldVm Model { get; set; } = default!;
    private bool _processing;
    private void Cancel() => MudDialog.Cancel();

    private void OnValidSubmit()
    {
        _processing = true;

        MudDialog.Close(DialogResult.Ok(Model));

        _processing = false;
    }
}
