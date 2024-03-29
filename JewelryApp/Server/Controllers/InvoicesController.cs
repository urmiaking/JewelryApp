﻿using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Application.Interfaces;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.Invoices;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetInvoiceListRequest request, CancellationToken cancellationToken)
        => Ok(await _invoiceService.GetInvoicesAsync(request, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _invoiceService.GetInvoiceByIdAsync(id, cancellationToken);

        return response.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddInvoiceRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _addInvoiceValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _invoiceService.AddInvoiceAsync(request, cancellationToken);

        return response.Match(x => CreatedAtAction(nameof(GetById), new { id = x.Id }, response.Value), Problem);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateInvoiceRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _updateInvoiceRequest.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _invoiceService.UpdateInvoiceAsync(request, cancellationToken);

        return response.Match(Ok, Problem);
    }

    [HttpDelete("{id:int}/{deletePermanently:bool}")]
    public async Task<IActionResult> Remove(int id, bool deletePermanently = false, CancellationToken cancellationToken = default)
    {
        var result = await _invoiceService.RemoveInvoiceAsync(id, deletePermanently, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpGet(nameof(Count))]
    public async Task<IActionResult> Count(CancellationToken cancellationToken)
        => Ok(await _invoiceService.GetTotalInvoicesCount(cancellationToken));

    [HttpGet(nameof(GetInvoiceNumber))]
    public async Task<IActionResult> GetInvoiceNumber(CancellationToken cancellationToken)
        => Ok(await _invoiceService.GetLastInvoiceNumber(cancellationToken));
}

