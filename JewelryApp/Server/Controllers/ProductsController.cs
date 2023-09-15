using JewelryApp.Business.AppServices;
using JewelryApp.Common.Enums;
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
    public async Task<IActionResult> Get()
        => Ok(await _productService.GetProductsAsync());

    [HttpPost]
    public async Task<IActionResult> AddOrEditProduct(SetProductDto productDto)
        => Ok(await _productService.SetProductAsync(productDto));
    

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken, int id = 0)
    {
        if (id == 0)
            return BadRequest();

        var deleteResult = await _productService.DeleteProductAsync(id, cancellationToken);

        return deleteResult switch
        {
            DeleteResult.CanNotDelete => ValidationProblem(),
            DeleteResult.IsNotAvailable => BadRequest(),
            _ => Ok(deleteResult)
        };
    }
}

