using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Requests.Customer;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

public class CustomersController : ApiController
{
    private readonly IValidator<AddCustomerRequest> _addCustomerValidator;
    private readonly IValidator<UpdateCustomerRequest> _updateCustomerValidator;
    private readonly ICustomerService _customerService;

    public CustomersController(
        IValidator<AddCustomerRequest> addCustomerValidator,
        IValidator<UpdateCustomerRequest> updateCustomerValidator,
        ICustomerService customerService)
    {
        _addCustomerValidator = addCustomerValidator;
        _updateCustomerValidator = updateCustomerValidator;
        _customerService = customerService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken token)
    {
        var response = await _customerService.GetCustomerByIdAsync(id, token);

        return response.Match(Ok, Problem);
    }

    [HttpGet("invoice/{invoiceId:int}")]
    public async Task<IActionResult> GetByInvoiceId(int invoiceId, CancellationToken token)
    {
        var response = await _customerService.GetCustomerByInvoiceIdAsync(invoiceId, token);

        return response.Match(Ok, Problem);
    }

    [HttpGet("phoneNumber/{phoneNumber}")]
    public async Task<IActionResult> GetByPhoneNumber(string phoneNumber, CancellationToken token)
    {
        var response = await _customerService.GetCustomerByPhoneNumberAsync(phoneNumber, token);
        
        return response.Match(Ok, Problem);
    }

    [HttpGet("nationalCode/{nationalCode}")]
    public async Task<IActionResult> GetByNationalCode(string nationalCode, CancellationToken token)
    {
        var response = await _customerService.GetCustomerByNationalCodeAsync(nationalCode, token);

        return response.Match(Ok, Problem);
    }

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

        return response.Match(x => CreatedAtAction(nameof(GetById), new { id = x.Id }, response.Value), Problem);
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

    [HttpDelete("{id:int}/{deletePermanently:bool}")]
    public async Task<IActionResult> Remove(int id, bool deletePermanently = false, CancellationToken token = default)
    {
        var response = await _customerService.RemoveCustomerAsync(id, deletePermanently, token);

        return response.Match(Ok, Problem);
    }
}
