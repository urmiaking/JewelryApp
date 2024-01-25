using JewelryApp.Client.ViewModels.Invoice;
using JewelryApp.Shared.Abstractions;
using Microsoft.AspNetCore.Components;

namespace JewelryApp.Client.Pages.Invoices;

public partial class ViewInvoice
{
    [Parameter] public int InvoiceId { get; set; }
    [Inject] private IInvoiceItemService InvoiceItemService { get; set; } = default!;
    [Inject] private IInvoiceService InvoiceService { get; set; } = default!;
    [Inject] private ICustomerService CustomerService { get; set; } = default!;
    [Inject] private IOldGoldService OldGoldService { get; set; } = default!;

    private List<ViewInvoiceItemVm> _items = new ();
    private ViewCustomerVm _customer = new ();
    private ViewInvoiceVm _invoice = new ();
    private ViewOldGoldsVm _oldGolds = new ();

    protected override async Task OnInitializedAsync()
    {
        await LoadCustomerAsync();
        await LoadInvoiceAsync();
        await LoadInvoiceItemsAsync();
        await LoadOldGoldsAsync();
        
        await base.OnInitializedAsync();
    }

    private async Task LoadOldGoldsAsync()
    {
        var response = await OldGoldService.GetOldGoldsByInvoiceIdAsync(InvoiceId, CancellationTokenSource.Token);

        if (!response.IsError)
            _oldGolds = Mapper.Map<ViewOldGoldsVm>(response.Value);
    }

    private async Task LoadInvoiceAsync()
    {
        var response = await InvoiceService.GetInvoiceByIdAsync(InvoiceId, CancellationTokenSource.Token);

        if (!response.IsError)
            _invoice = Mapper.Map<ViewInvoiceVm>(response.Value);
    }

    private async Task LoadInvoiceItemsAsync()
    {
        var invoiceItems = await InvoiceItemService.GetInvoiceItemsByInvoiceIdAsync(InvoiceId, CancellationTokenSource.Token);

        if (!invoiceItems.IsError)
            _items = Mapper.Map<List<ViewInvoiceItemVm>>(invoiceItems.Value);
    }

    private async Task LoadCustomerAsync()
    {
        var response = await CustomerService.GetCustomerByInvoiceIdAsync(InvoiceId, CancellationTokenSource.Token);

        if (!response.IsError)
            _customer = Mapper.Map<ViewCustomerVm>(response.Value);
    }
}