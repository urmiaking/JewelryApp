using JewelryApp.Client.Pages.Components.Product;
using JewelryApp.Client.Shared;
using JewelryApp.Common.Enums;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http;
using System.Net.Http.Json;
using static MudBlazor.CategoryTypes;

namespace JewelryApp.Client.Pages.Components.Invoice;

public partial class InvoicesTable
{
    [Inject] public IDialogService Dialog { get; set; } = default!;
    
    private IEnumerable<InvoiceTableItemDto> _pagedData = new List<InvoiceTableItemDto>();
    private MudTable<InvoiceTableItemDto> _table = new();

    private int totalItems;
    private string searchString = null;

    private async Task<TableData<InvoiceTableItemDto>> ServerReload(TableState state)
    {
        await LoadData();
        _invoices = _invoices.Where(invoice =>
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (invoice.BuyerName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (invoice.BuyerPhone.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (invoice.BuyDate.Contains(searchString))
                return true;
            return false;
        }).ToList();
        totalItems = _invoices.Count;
        _invoices = state.SortLabel switch
        {
            "index_field" => _invoices.OrderByDirection(state.SortDirection, o => o.Index).ToList(),
            "id_field" => _invoices.OrderByDirection(state.SortDirection, o => o.InvoiceId).ToList(),
            "name_field" => _invoices.OrderByDirection(state.SortDirection, o => o.BuyerName).ToList(),
            "phone_field" => _invoices.OrderByDirection(state.SortDirection, o => o.BuyerPhone).ToList(),
            "cost_field" => _invoices.OrderByDirection(state.SortDirection, o => o.TotalCost).ToList(),
            "count_field" => _invoices.OrderByDirection(state.SortDirection, o => o.ProductsCount).ToList(),
            "date_field" => _invoices.OrderByDirection(state.SortDirection, o => o.BuyDate).ToList(),
            _ => _invoices
        };

        _pagedData = _invoices.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new TableData<InvoiceTableItemDto>() { TotalItems = totalItems, Items = _pagedData };
    }

    private void OnSearch(string text)
    {
        searchString = text;
        _table.ReloadServerData();
    }

    [Parameter]
    public string? Class { get; set; }

    private List<InvoiceTableItemDto>? _invoices;

    DialogOptions _dialogOptions = new() { CloseButton = true, FullWidth = true, FullScreen = false };
    
    private async Task LoadData()
    {
        _invoices = await GetAsync<List<InvoiceTableItemDto>>("/api/Invoices") ?? new List<InvoiceTableItemDto>();
        StateHasChanged();
    }

    private async Task CommitItemAsync(object elemnt)
    {
        var invoiceItemDto = elemnt as InvoiceTableItemDto;

        //var productDto = new SetProductDto
        //{
        //    Id = invoiceItemDto.Id,
        //    Name = invoiceItemDto.Name,
        //    Caret = invoiceItemDto.Caret,
        //    ProductType = invoiceItemDto.ProductType,
        //    Wage = invoiceItemDto.Wage,
        //    Weight = invoiceItemDto.Weight,
        //    BarcodeText = invoiceItemDto.BarcodeText
        //};

        //await PostAsync("/api/Invoices", productDto);
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

            if (!isDeleted)
            {
                SnackBar.Add("حذف با خطا مواجه شد");
            }
        }

        await _table.ReloadServerData();
    }

    private void EditInvoice(int contextInvoiceId)
    {
        NavigationManager.NavigateTo($"/invoices/edit/{contextInvoiceId}");
    }
}

