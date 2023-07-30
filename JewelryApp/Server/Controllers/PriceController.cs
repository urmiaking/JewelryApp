using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Data;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PriceController : ControllerBase
{
    private readonly IApiPrice _apiPrice;

    public PriceController(IApiPrice apiPrice)
    {
        _apiPrice = apiPrice;
    }

    [HttpGet]
    public async Task<IActionResult> GetPrice()
    {
        try
        {
            var result = new PriceModel
            {
                Price = await _apiPrice.GetGramPrice()
            };

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}

