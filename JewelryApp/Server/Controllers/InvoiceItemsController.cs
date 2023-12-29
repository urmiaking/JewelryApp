using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Application.Interfaces;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.InvoiceItems;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

public class InvoiceItemsController : ApiController
{
    private readonly IValidator<AddInvoiceItemRequest> _addInvoiceItemValidator;
    private readonly IValidator<UpdateInvoiceItemRequest> _updateInvoiceItemValidator;
    private readonly IInvoiceItemService _invoiceItemService;

    public InvoiceItemsController(IValidator<AddInvoiceItemRequest> addInvoiceItemValidator, IValidator<UpdateInvoiceItemRequest> updateInvoiceItemValidator,
        IInvoiceItemService invoiceItemService)
    {
        _addInvoiceItemValidator = addInvoiceItemValidator;
        _updateInvoiceItemValidator = updateInvoiceItemValidator;
        _invoiceItemService = invoiceItemService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken token)
    {
        var response = await _invoiceItemService.GetByIdAsync(id, token);

        return response.Match(Ok, Problem);
    }

    [HttpGet("invoice/{invoiceId:int}")]
    public async Task<IActionResult> GetInvoiceItems(int invoiceId, CancellationToken token)
    {
        var response = await _invoiceItemService.GetInvoiceItemsByInvoiceIdAsync(invoiceId, token);

        return response.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddInvoiceItemRequest request, CancellationToken token)
    {
        var validationResult = await _addInvoiceItemValidator.ValidateAsync(request, token);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _invoiceItemService.AddInvoiceItemAsync(request, token);

        return response.Match(x => CreatedAtAction(nameof(GetById), new { id = x.Id }, response.Value), Problem);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateInvoiceItemRequest request, CancellationToken token)
    {
        var validationResult = await _updateInvoiceItemValidator.ValidateAsync(request, token);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _invoiceItemService.UpdateInvoiceItemAsync(request, token);

        return response.Match(Ok, Problem);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remove(int id, CancellationToken token)
    {
        var response = await _invoiceItemService.RemoveInvoiceItemAsync(id, token);

        return response.Match(Ok, Problem);
    } 
}
