using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Application.Interfaces;
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

    [HttpGet(nameof(GetInvoiceItems))]
    public async Task<IActionResult> GetInvoiceItems(GetInvoiceItemsRequest request, CancellationToken token)
        => Ok(await _invoiceItemService.GetInvoiceItemsAsync(request, token));

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

        return response.Match(Ok, Problem);
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

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveInvoiceItemRequest request, CancellationToken token)
        => Ok(await _invoiceItemService.RemoveInvoiceItemAsync(request, token));  
}
