using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Application.Interfaces;
using JewelryApp.Shared.Requests.Customer;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

public class CustomersController : ApiController
{
    private readonly IValidator<AddCustomerRequest> _addCustomerValidator;
    private readonly IValidator<UpdateCustomerRequest> _updateCustomerValidator;
    private readonly ICustomerService _customerService;

    public CustomersController(IValidator<AddCustomerRequest> addCustomerValidator, IValidator<UpdateCustomerRequest> updateCustomerValidator, ICustomerService customerService)
    {
        _addCustomerValidator = addCustomerValidator;
        _updateCustomerValidator = updateCustomerValidator;
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(GetCustomerRequest request, CancellationToken token)
        => Ok(await _customerService.GetCustomerByInvoiceIdAsync(request, token));

    [HttpPost]
    public async Task<IActionResult> Add(AddCustomerRequest request, CancellationToken token)
    {
        var validationResult = await _addCustomerValidator.ValidateAsync(request, token);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _customerService.AddCustomerAsync(request, token);

        return response.Match(Ok, Problem);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCustomerRequest request, CancellationToken token)
    {
        var validationResult = await _updateCustomerValidator.ValidateAsync(request, token);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _customerService.UpdateCustomerAsync(request, token);

        return response.Match(Ok, Problem);
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveCustomerRequest request, CancellationToken token)
        => Ok(await _customerService.RemoveCustomerAsync(request, token));
}
