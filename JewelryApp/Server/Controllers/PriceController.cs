using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PriceController : ControllerBase
{
    private readonly IPriceRepository _priceRepository;

    public PriceController(IPriceRepository priceRepository)
    {
        _priceRepository = priceRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _priceRepository.GetPriceAsync();

        if (result is null)
            return BadRequest();

        //await _priceRepository.AddPriceAsync(result);

        return Ok(result);
    }

    [HttpGet(nameof(GetCaretData))]
    public async Task<IActionResult> GetCaretData(CaretChartType caretChartType)
    {
        var result = await _priceRepository.GetCaretChartDataAsync(caretChartType);

        return Ok(result);
    }
}

