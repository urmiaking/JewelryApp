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
        => Ok(await _priceService.GetPriceAsync());
}