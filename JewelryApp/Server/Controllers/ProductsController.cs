using JewelryApp.Business.AppServices;
using JewelryApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int page, int pageSize, string sortDirection, string? sortLabel, string? searchString, CancellationToken cancellationToken)
        => Ok(await _productService.GetProductsAsync(page, pageSize, sortDirection, sortLabel, searchString, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> AddOrEditProduct(SetProductDto productDto)
        => Ok(await _productService.SetProductAsync(productDto));
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken, int id = 0)
        => Ok(await _productService.DeleteProductAsync(id, cancellationToken));

    [HttpGet(nameof(GetTotalProductsCount))]
    public async Task<IActionResult> GetTotalProductsCount(CancellationToken cancellationToken)
        => Ok(await _productService.GetTotalProductsCount(cancellationToken));

    [HttpGet("{barcodeText}")]
    public async Task<IActionResult> GetByBarcode(string barcodeText)
        => Ok(await _productService.GetProductByBarcodeAsync(barcodeText));
}

