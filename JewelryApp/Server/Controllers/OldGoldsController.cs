using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.OldGolds;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

public class OldGoldsController : ApiController
{
    private readonly IOldGoldService _oldGoldService;
    private readonly IValidator<AddOldGoldRequest> _addOldGoldValidator;

    public OldGoldsController(IOldGoldService oldGoldService, IValidator<AddOldGoldRequest> addOldGoldValidator)
    {
        _oldGoldService = oldGoldService;
        _addOldGoldValidator = addOldGoldValidator;
    }

    [HttpGet("{invoiceId:int}")]
    public async Task<IActionResult> Get(int invoiceId, CancellationToken cancellationToken)
    {
        var response = await _oldGoldService.GetOldGoldsByInvoiceIdAsync(invoiceId, cancellationToken);

        return response.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddOldGoldRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _addOldGoldValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _oldGoldService.AddOldGoldAsync(request, cancellationToken);

        return response.Match(Ok, Problem);
    }

    [HttpDelete("{id:int}/{deletePermanently:bool}")]
    public async Task<IActionResult> Remove(int id, bool deletePermanently = false, CancellationToken cancellationToken = default)
    {
        var response = await _oldGoldService.RemoveOldGoldAsync(id, deletePermanently, cancellationToken);

        return response.Match(Ok, Problem);
    }
}

