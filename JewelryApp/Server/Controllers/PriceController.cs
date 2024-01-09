using Microsoft.AspNetCore.Mvc;
using JewelryApp.Shared.Abstractions;

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