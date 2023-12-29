using Microsoft.AspNetCore.Mvc;
using JewelryApp.Application.Interfaces;
using JewelryApp.Shared.Abstractions;
using Errors = JewelryApp.Shared.Errors.Errors;

namespace JewelryApp.Api.Controllers;

public class PriceController : ApiController
{
    private readonly IPriceService _priceService;

    public PriceController(IPriceService priceService)
    {
        _priceService = priceService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _priceService.GetPriceAsync();

        if (response.IsError && response.FirstError == Errors.General.NoInternet)
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: response.FirstError.Description);

        return response.Match(Ok, Problem);
    }
}