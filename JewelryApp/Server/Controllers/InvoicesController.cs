using JewelryApp.Common.DateFunctions;
using JewelryApp.Data;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly AppDbContext _context;

    public InvoicesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetInvoices(int count = 0)
    {
        var query = _context.Invoices.Include(a => a.InvoiceProducts).OrderByDescending(a => a.BuyDateTime);

        query = count == 0 ? query : query.Take(count).OrderByDescending(a => a.BuyDateTime);

        var result = (await query.ToListAsync()).Select((a, i) => new InvoiceTableItemDto()
        {
            InvoiceId = a.Id,
            Index = i + 1,
            BuyDate = a.BuyDateTime.ToShamsiDateString(),
            BuyerName = a.BuyerFirstName + " " + a.BuyerLastName,
            BuyerPhone = a.BuyerPhoneNumber,
            ProductsCount = a.InvoiceProducts.Count,
            TotalCost = a.InvoiceProducts.Sum(b => b.FinalPrice)
        });

        return Ok(result);
    }
}

