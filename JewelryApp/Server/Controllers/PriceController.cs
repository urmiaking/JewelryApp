using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Data;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PriceController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPriceRepository _priceRepository;

    public PriceController(AppDbContext context, IPriceRepository priceRepository)
    {
        _context = context;
        _priceRepository = priceRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetPriceModel()
    {
        try
        {
            var result = new PriceModel
            {
                LastUpdate = await _priceRepository.GetLastUpdateTime(),
                Price = await _priceRepository.GetGramPrice()
            };

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}

