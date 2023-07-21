using JewelryApp.Data;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(int count = 0)
    {
        var query = _context.Products.OrderByDescending(a => a.AddedDateTime);

        query = count == 0 ? query : query.Take(count).OrderByDescending(a => a.AddedDateTime);

        var result = (await query.ToListAsync()).Select((a, i) => new ProductTableItemDto
        {
            BarcodeText = a.BarcodeText,
            Id = a.Id,
            Index = i + 1,
            Name = a.Name,
            ProductType = a.ProductType
        });

        return Ok(result);
    }
}

