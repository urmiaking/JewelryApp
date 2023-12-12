using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Application.Interfaces;
using JewelryApp.Shared.Requests.Invoices;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ApiController
{
    private readonly IInvoiceService _invoiceService;
    private readonly IValidator<AddInvoiceRequest> _addInvoiceValidator;
    private readonly IValidator<UpdateInvoiceRequest> _updateInvoiceRequest;

    public InvoicesController(IInvoiceService invoiceService, IValidator<AddInvoiceRequest> addInvoiceValidator, IValidator<UpdateInvoiceRequest> updateInvoiceRequest)
    {
        _invoiceService = invoiceService;
        _addInvoiceValidator = addInvoiceValidator;
        _updateInvoiceRequest = updateInvoiceRequest;
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
        var validationResult = await _addInvoiceValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var addResult = await _invoiceService.AddInvoiceAsync(request, cancellationToken);

        return addResult.Match(Ok, Problem);
    }

    [HttpPut(nameof(Update))]
    public async Task<IActionResult> Update(UpdateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _updateInvoiceRequest.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var updateResult = await _invoiceService.UpdateInvoiceAsync(request, cancellationToken);

        return updateResult.Match(Ok, Problem);
    }

    [HttpDelete(nameof(Remove))]
    public async Task<IActionResult> Remove(RemoveInvoiceRequest request, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.RemoveInvoiceAsync(request, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpGet(nameof(GetTotalInvoicesCount))]
    public async Task<IActionResult> GetTotalInvoicesCount(CancellationToken cancellationToken)
        => Ok(await _invoiceService.GetTotalInvoicesCount(cancellationToken));
}

