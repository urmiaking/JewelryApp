using AutoMapper;
using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Data;
using JewelryApp.Data.Models;
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
    private readonly IMapper _mapper;
    private readonly IBarcodeRepository _barcodeRepository;

    public ProductsController(AppDbContext context, IMapper mapper, IBarcodeRepository barcodeRepository)
    {
        _context = context;
        _mapper = mapper;
        _barcodeRepository = barcodeRepository;
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
            ProductType = a.ProductType,
            Caret = a.Caret,
            Wage = a.Wage,
            Weight = a.Weight
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrEditProduct(AddProductDto productDto)
    {
        var productModel = _mapper.Map<AddProductDto, Product>(productDto);

        // Add
        if (productDto.Id == 0)
        {
            productModel.AddedDateTime = DateTime.Now;
            productModel.BarcodeText = await _barcodeRepository.GetBarcodeAsync(productModel);
            
            await _context.Products.AddAsync(productModel);
        }
        // Update
        else
        {
            var productDb = await _context.Products.FirstOrDefaultAsync(a => a.Id == productDto.Id);

            if (productDb is not null)
            {
                productDb.ProductType = productModel.ProductType;
                productDb.Caret = productModel.Caret;
                productDb.Name = productModel.Name;
                productDb.Wage = productModel.Wage;
                productDb.Weight = productModel.Weight;

                _context.Products.Update(productDb);
            }
            else
            {
                return BadRequest();
            }
        }

        await _context.SaveChangesAsync();

        return Ok();
    }
}

