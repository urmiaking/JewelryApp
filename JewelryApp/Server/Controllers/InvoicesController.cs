using JewelryApp.Application.Interfaces;
using JewelryApp.Models.Dtos.Invoice;
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

    [HttpGet]
    public async Task<IActionResult> Get(int page, int pageSize, string sortDirection, string? sortLabel, string? searchString, CancellationToken cancellationToken)
        => Ok(await _invoiceService.GetInvoicesAsync(page, pageSize, sortDirection, sortLabel, searchString, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInvoice(int id, CancellationToken cancellationToken) =>
        Ok(await _invoiceService.GetInvoiceAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Set(InvoiceDto invoiceDto, CancellationToken cancellationToken)
    {
        var succeed = await _invoiceService.SetInvoiceAsync(invoiceDto, cancellationToken);
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

