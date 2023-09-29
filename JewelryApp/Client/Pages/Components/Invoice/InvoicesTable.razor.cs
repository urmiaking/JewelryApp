using JewelryApp.Client.Shared;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.Invoice;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JewelryApp.Client.Pages.Components.Invoice;

public partial class InvoicesTable
{
    [Inject] 
    public IDialogService Dialog { get; set; } = default!;

    [Parameter]
    public string? Class { get; set; }

    private List<InvoiceTableItemDto> _invoices = new();

    private DialogOptions _dialogOptions = new() { CloseButton = true, FullWidth = true, FullScreen = false };

    private MudTable<InvoiceTableItemDto> _table = new();

    private int _totalItems;
    private string? _searchString;

    private async Task<TableData<InvoiceTableItemDto>> ServerReload(TableState state)
    {
        await LoadData(state);

        return new TableData<InvoiceTableItemDto> { TotalItems = _totalItems, Items = _invoices };
    }

    private void OnSearch(string text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }

    private async Task LoadData(TableState state)
    {
        _invoices = await GetAsync<List<InvoiceTableItemDto>>(
            $"/api/Invoices?page={state.Page}&pageSize={state.PageSize}&sortDirection={state.SortDirection}&sortLabel={state.SortLabel}&searchString={_searchString}") 
                    ?? new List<InvoiceTableItemDto>();

        _totalItems = await GetAsync<int>("/api/Invoices/GetTotalInvoicesCount");

        StateHasChanged();
    }

    private async Task CommitItemAsync(object element)
    {
        if (element is InvoiceTableItemDto invoiceItemDto)
        {
            var invoiceHeader = new InvoiceHeaderDto
            {
                InvoiceId = invoiceItemDto.InvoiceId,
                CustomerName = invoiceItemDto.CustomerName,
                CustomerPhone = invoiceItemDto.CustomerPhone
            };

            await PostAsync("/api/Invoices/UpdateInvoiceHeader", invoiceHeader);
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