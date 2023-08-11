using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;

namespace JewelryApp.Client.Pages.Components.Invoice;

public partial class InvoicesTable
{
    [Parameter]
    public string? Class { get; set; }
    
    [Parameter]
    public bool IsInInvoicePage { get; set; }

    [Parameter]
    public bool Filterable { get; set; }

    private IEnumerable<InvoiceTableItemDto>? _invoices;

    protected override async Task OnInitializedAsync()
    {
        _invoices = await AuthorizedHttpClient.GetFromJsonAsync<IEnumerable<InvoiceTableItemDto>>($"/api/Invoices/GetInvoices");
    }
}

