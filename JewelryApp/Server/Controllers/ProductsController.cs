using FluentValidation;
using JewelryApp.Api.Common.Extensions;
using JewelryApp.Application.Interfaces;
using JewelryApp.Shared.Requests.Products;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

public class ProductsController : ApiController
{
    private readonly IProductService _productService;
    private readonly IValidator<AddProductRequest> _addProductValidator;
    private readonly IValidator<UpdateProductRequest> _updateProductValidator;

    public ProductsController(IProductService productService, IValidator<AddProductRequest> addProductValidator, IValidator<UpdateProductRequest> updateProductValidator)
    {
        _productService = productService;
        _addProductValidator = addProductValidator;
        _updateProductValidator = updateProductValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetProductsRequest request, CancellationToken cancellationToken) 
        => Ok(await _productService.GetProductsAsync(request, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductByIdAsync(id, cancellationToken);
        return response.Match(Ok, Problem);
    }
    
    [HttpGet("barcode/{barcode}")]
    public async Task<IActionResult> GetByBarcode(string barcode, CancellationToken cancellationToken)
    {
        var response = await _productService.GetProductByBarcodeAsync(barcode, cancellationToken);
        return response.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddProductRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _addProductValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _productService.AddProductAsync(request, cancellationToken);
        return response.Match(Ok, Problem);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _updateProductValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        var response = await _productService.UpdateProductAsync(request, cancellationToken);
        return response.Match(Ok, Problem);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remove(int id, CancellationToken cancellationToken)
    {
        var response = await _productService.RemoveProductAsync(id, cancellationToken);
        return response.Match(Ok, Problem);
    }

    [HttpGet(nameof(Count))]
    public async Task<IActionResult> Count(CancellationToken cancellationToken)
        => Ok(await _productService.GetTotalProductsCount(cancellationToken));
}