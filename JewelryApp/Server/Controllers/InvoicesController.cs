using AutoMapper;
using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Common.DateFunctions;
using JewelryApp.Data;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IBarcodeRepository _barcodeRepository;

    public InvoicesController(AppDbContext context, IMapper mapper, IBarcodeRepository barcodeRepository)
    {
        _context = context;
        _mapper = mapper;
        _barcodeRepository = barcodeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = _context.Invoices
            .Include(a => a.InvoiceProducts)
            .OrderByDescending(a => a.BuyDateTime);

        var result = (await query.ToListAsync()).Select((a, i) => new InvoiceTableItemDto
        {
            InvoiceId = a.Id,
            Index = i + 1,
            BuyDate = a.BuyDateTime.HasValue ? a.BuyDateTime.Value.ToShamsiDateString() : "",
            BuyerName = a.BuyerFirstName + " " + a.BuyerLastName,
            BuyerPhone = a.BuyerPhoneNumber,
            ProductsCount = a.InvoiceProducts.Count,
            TotalCost = a.InvoiceProducts.Sum(b => b.FinalPrice)
        });

        return Ok(result);
    }

    [HttpGet(nameof(GetInvoice))]
    public async Task<IActionResult> GetInvoice(int id = 0)
    {
        if (id is 0)
            return BadRequest();

        var invoice = await _context.Invoices.FirstOrDefaultAsync(x => x.Id == id);

        if (invoice is null)
            return BadRequest();

        var invoiceDto = _mapper.Map<Invoice, InvoiceDto>(invoice);
        return Ok(invoiceDto);
    }

    [HttpPost]
    public async Task<IActionResult> Set(InvoiceDto invoiceDto)
    {
        var invoice = _mapper.Map<InvoiceDto, Invoice>(invoiceDto);

        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();
        

        foreach (var productDto in invoiceDto.Products)
        {
            var product = _mapper.Map<ProductDto, Product>(productDto);
            productDto.GramPrice = invoiceDto.GramPrice;
            // New Product
            if (product.Id == 0)
            {
                product.BarcodeText = await _barcodeRepository.GetBarcodeAsync(product);
                product.AddedDateTime = DateTime.Now;
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }

            var invoiceProduct = new InvoiceProduct
            {
                InvoiceId = invoice.Id,
                ProductId = product.Id,
                Invoice = invoice,
                Product = product,
                Count = productDto.Count,
                Profit = productDto.Profit / 100.0,
                GramPrice = invoiceDto.GramPrice,
                TaxOffset = productDto.TaxOffset / 100.0,
                FinalPrice = productDto.FinalPrice,
                Tax = productDto.Tax
            };

            _context.Entry(product).State = EntityState.Unchanged;
            await _context.InvoiceProducts.AddAsync(invoiceProduct);
            await _context.SaveChangesAsync();
        }

        return Ok();
    }
}

