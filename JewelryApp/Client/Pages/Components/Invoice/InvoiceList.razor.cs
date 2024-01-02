using JewelryApp.Client.Shared;
using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.Invoices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Invoice;

public partial class InvoiceList
{
    [Inject] public IDialogService Dialog { get; set; } = default!;
    [Inject] public IInvoiceService InvoiceService { get; set; } = default!;

    [Parameter] public string? Class { get; set; }

    private List<InvoicesListVm> _invoicesList = new();

    private readonly DialogOptions _dialogOptions = new() { CloseButton = true, FullWidth = true, FullScreen = false };

    private MudTable<InvoicesListVm> _table = new();

    private int _totalItems;
    private string? _searchString;

    private async Task<TableData<InvoicesListVm>> ServerReload(TableState state)
    {
        await LoadData(state);

        return new TableData<InvoicesListVm> { TotalItems = _totalItems, Items = _invoicesList };
    }

    private void OnSearch(string text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }

    private async Task LoadData(TableState state)
    {
        var request = new GetInvoiceListRequest(state.Page, state.PageSize, state.SortDirection.ToDescriptionString(), state.SortLabel, _searchString);

        var invoices = await InvoiceService.GetInvoicesAsync(request, CancellationTokenSource.Token);

        _invoicesList = Mapper.Map<List<InvoicesListVm>>(invoices);

        var itemCountResponse = await InvoiceService.GetTotalInvoicesCount(CancellationTokenSource.Token);

        _totalItems = itemCountResponse.Count;

        StateHasChanged();
    }

    private async Task CommitItemAsync(object element)
    {
        if (element is InvoicesListVm invoiceItemDto)
        {
            var request = Mapper.Map<UpdateInvoiceRequest>(invoiceItemDto);

            await InvoiceService.UpdateInvoiceAsync(request, CancellationTokenSource.Token);
        }
    }

    private async Task DeleteInvoice(DialogOptions options, int invoiceId)
    {
        var parameters = new DialogParameters<PromptDialog>
        {
            { x => x.ContentText, "آیا مطمئن هستید؟ اطلاعات فاکتور حذف شده غیر قابل بازیابی خواهد بود" },
            { x => x.ButtonText, "حذف" },
            { x => x.Color, Color.Error },
            { x => x.EndIcon, Icons.Material.Filled.Delete },
            { x => x.EndpointUrl, $"/api/Invoices/{invoiceId}"}
        };

        var dialog = await Dialog.ShowAsync<PromptDialog>("حذف فاکتور", parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var isDeleted = (bool)result.Data;

            if (isDeleted)
            {
                await _table.ReloadServerData();
            }
        }
    }

    private void EditInvoice(int contextInvoiceId)
    {
        NavigationManager.NavigateTo($"/invoices/edit/{contextInvoiceId}");
    }

    private void PageChanged(int i)
    {
        _table.NavigateTo(i - 1);
    }
}