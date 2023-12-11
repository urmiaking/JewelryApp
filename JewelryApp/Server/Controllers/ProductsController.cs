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
    public async Task<IActionResult> GetAll(GetProductsRequest request, CancellationToken cancellationToken)
    {
        var products = await _productService.GetProductsAsync(request, cancellationToken);

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(AddProductRequest request, CancellationToken cancellationToken)
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
    public async Task<IActionResult> UpdateProduct(UpdateProductRequest request, CancellationToken cancellationToken)
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveProduct(RemoveProductRequest request, CancellationToken cancellationToken)
    {
        var response = await _productService.RemoveProductAsync(request, cancellationToken);

        return response.Match(Ok, Problem);
    }

    [HttpGet(nameof(GetTotalProductsCount))]
    public async Task<IActionResult> GetTotalProductsCount(CancellationToken cancellationToken)
    {
        return Ok(await _productService.GetTotalProductsCount(cancellationToken));
    }

    [HttpGet("{barcodeText}")]
    public async Task<IActionResult> GetByBarcode(string barcode, CancellationToken cancellationToken)
    {
        return Ok(await _productService.GetProductByBarcodeAsync(barcode, cancellationToken));
    }
}