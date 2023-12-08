using JewelryApp.Business.Interfaces;
using JewelryApp.Common.Errors;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        try
        {
            var response = await _priceService.GetPriceAsync();

            if (response.IsError && response.FirstError == Errors.General.NoInternet)
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: response.FirstError.Description);

            return response.Match(Ok, Problem);
        }
        catch (ValidationException e)
        {
            return ValidationProblem(title: e.Message);
        }
        catch (Exception e)
        {
            return Problem(title: e.Message);
        }
    }
}