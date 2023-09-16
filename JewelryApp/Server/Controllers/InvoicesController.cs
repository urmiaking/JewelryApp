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

    [HttpPost]
    public async Task<IActionResult> Set(InvoiceDto invoiceDto)
    {
        var invoice = _mapper.Map<InvoiceDto, Invoice>(invoiceDto);

        foreach (var productDto in invoiceDto.Products)
        {
            var invoiceProduct = _mapper.Map<ProductDto, InvoiceProduct>(productDto);

            if (productDto.Id == 0)
            {
                var newProduct = _mapper.Map<ProductDto, Product>(productDto);
                newProduct.BarcodeText = await _barcodeRepository.GetBarcodeAsync(newProduct);
                newProduct.AddedDateTime = DateTime.Now;

                await _context.Products.AddAsync(newProduct);
                await _context.SaveChangesAsync();
                productDto.Id = newProduct.Id;
            }

            invoiceProduct.ProductId = productDto.Id;
            invoice.InvoiceProducts.Add(invoiceProduct);
        }

        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();

        return Ok();
    }
}

