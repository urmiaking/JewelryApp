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
    private readonly IMapper _mapper;
    private readonly IBarcodeRepository _barcodeRepository;
    private readonly IRepository<Product> _productRepository;

    public ProductsController(IRepository<Product> productRepository, IMapper mapper, IBarcodeRepository barcodeRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
        _barcodeRepository = barcodeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(int count = 0)
    {
        var query = _productRepository.Entities.OrderByDescending(a => a.AddedDateTime);

        query = count == 0 ? query : query.Take(count).OrderByDescending(a => a.AddedDateTime);

        var products = _mapper.Map<List<Product>, List<ProductTableItemDto>>((await query.ToListAsync()));

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrEditProduct(AddProductDto productDto, CancellationToken cancellationToken)
    {
        var productModel = _mapper.Map<AddProductDto, Product>(productDto);

        // Add
        if (productDto.Id == 0)
        {
            productModel.AddedDateTime = DateTime.Now;
            productModel.BarcodeText = await _barcodeRepository.GetBarcodeAsync(productModel);
            
            await _productRepository.AddAsync(productModel, cancellationToken);
        }
        // Update
        else
        {
            var productDb = await _productRepository.GetByIdAsync(cancellationToken, productDto.Id);

            if (productDb is not null)
            {
                productDb.ProductType = productModel.ProductType;
                productDb.Caret = productModel.Caret;
                productDb.Name = productModel.Name;
                productDb.Wage = productModel.Wage;
                productDb.Weight = productModel.Weight;

                await _productRepository.UpdateAsync(productDb, cancellationToken);
            }
            else
            {
                return BadRequest();
            }
        }

        return Ok();
    }
}

