using JewelryApp.Application.Interfaces;
using JewelryApp.Shared.Requests.Invoices;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll(GetInvoiceTableRequest request, CancellationToken cancellationToken)
        => Ok(await _invoiceService.GetInvoicesAsync(request, cancellationToken));

    [HttpGet(nameof(Get))]
    public async Task<IActionResult> Get(GetInvoiceRequest request, CancellationToken cancellationToken) =>
        Ok(await _invoiceService.GetInvoiceAsync(request, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Add(AddInvoiceRequest request, CancellationToken cancellationToken)
    {
        // TODO: Add validation
        var succeed = await _invoiceService.AddInvoiceAsync(request, cancellationToken);
        return succeed ? Ok() : BadRequest();
    }

    [HttpPost(nameof(UpdateInvoiceHeader))]
    public async Task<IActionResult> UpdateInvoiceHeader(InvoiceHeaderDto invoiceHeaderDto, CancellationToken cancellationToken)
        => Ok(await _invoiceService.UpdateInvoiceHeaderAsync(invoiceHeaderDto, cancellationToken));
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) => 
        Ok(await _invoiceService.DeleteAsync(id, cancellationToken));

    [HttpGet(nameof(GetTotalInvoicesCount))]
    public async Task<IActionResult> GetTotalInvoicesCount(CancellationToken cancellationToken)
        => Ok(await _invoiceService.GetTotalInvoicesCount(cancellationToken));
}

